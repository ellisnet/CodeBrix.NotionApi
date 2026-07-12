========================================================================
                  AGENT-README: CodeBrix.NotionApi
             A Comprehensive Guide for AI Coding Agents
========================================================================

OVERVIEW
------------------------------------------------------------------------
CodeBrix.NotionApi is a fully managed .NET client library for the Notion
API — a CodeBrix vendored replacement for the Notion.Net NuGet package
(version 5.0.0), adapted from the notion-sdk-net project and rebuilt on
System.Text.Json with no Newtonsoft.Json or JsonSubTypes dependencies.
Complete open-source licensing, attribution, and provenance information
for the adapted source is in THIRD-PARTY-NOTICES.txt at the root of this
repository. This repository builds TWO assemblies that ship together in
ONE NuGet package:

  * CodeBrix.NotionApi         — the Notion API client (ported code)
  * CodeBrix.JsonPolymorphism  — discriminator-based polymorphic JSON
    deserialization with fallback types, on top of System.Text.Json
    (original CodeBrix code; replaces the JsonSubTypes dependency)

The single-package/two-assembly layout is a deliberate, documented
deviation from the CodeBrix one-csproj-one-nupkg norm:
CodeBrix.JsonPolymorphism.csproj is IsPackable=false and its .dll/.xml
are placed into the package's lib/net10.0/ by a
TargetsForTfmSpecificBuildOutput hook in CodeBrix.NotionApi.csproj.

INSTALLATION
------------------------------------------------------------------------
NuGet package:  CodeBrix.NotionApi.MitLicenseForever

  dotnet add package CodeBrix.NotionApi.MitLicenseForever

Note that the package name carries the ".MitLicenseForever" suffix, but
the namespaces do NOT — code uses "CodeBrix.NotionApi" and
"CodeBrix.JsonPolymorphism". Target framework: .NET 10.0 or higher.

Package dependencies (all Microsoft-maintained):
  Microsoft.Extensions.Logging.Abstractions
  Microsoft.Extensions.DependencyInjection.Abstractions
  Microsoft.Extensions.Http

KEY NAMESPACES
------------------------------------------------------------------------
  using CodeBrix.NotionApi;         // the entire Notion client surface
                                    // (single flat namespace, like the
                                    // upstream Notion.Client namespace)
  using CodeBrix.JsonPolymorphism;  // JsonDiscriminator / JsonKnownType /
                                    // JsonFallbackType attributes and the
                                    // FallbackTypeConverter machinery

CORE API REFERENCE — CodeBrix.NotionApi
------------------------------------------------------------------------
Entry point:

  var client = NotionClientFactory.Create(new ClientOptions
  {
      AuthToken = "ntn_your_integration_token",
      // BaseUrl, NotionVersion, RetryPolicy, HttpClient are optional
  });

INotionClient exposes one sub-client per Notion endpoint group:
  client.Users            IUsersClient          (MeAsync, RetrieveAsync, ListAsync)
  client.Pages            IPagesClient          (CreateAsync, RetrieveAsync, UpdateAsync, ...)
  client.Databases        IDatabasesClient      (CreateAsync, QueryAsync, UpdateAsync, ...)
  client.DataSources      IDataSourcesClient    (RetrieveAsync, QueryAsync, UpdateAsync, ...)
  client.Blocks           IBlocksClient         (RetrieveAsync, RetrieveChildrenAsync, AppendChildrenAsync, ...)
  client.Search           ISearchClient         (SearchAsync)
  client.Comments         ICommentsClient       (CreateAsync, RetrieveAsync)
  client.FileUploads      IFileUploadsClient    (CreateAsync, SendAsync, ListAsync, RetrieveAsync)
  client.AuthenticationClient IAuthenticationClient (OAuth token exchange/introspection/refresh)
  client.RestClient       IRestClient           (low-level HTTP access)

Dependency injection: services.AddNotionClient(options => { ... })
registers INotionClient as a singleton with an IHttpClientFactory-managed
HttpClient (ServiceCollectionExtensions).

Error model: non-success responses throw NotionApiException (StatusCode,
NotionAPIErrorCode, Message); HTTP 429 throws NotionApiRateLimitException
with the Retry-After delay. A configurable IRetryPolicy
(DefaultRetryPolicy) retries 429 for all methods and 5xx for
GET/DELETE.

Serialization: all JSON goes through System.Text.Json using
RestClient.DefaultSerializerOptions (camelCase property naming,
WhenWritingNull ignore condition, case-insensitive property matching,
plus an internal RuntimeTypeConverterFactory that serializes
abstract/interface-declared values via their runtime type — matching
Newtonsoft.Json's behavior that the upstream code was designed around).

CORE API REFERENCE — CodeBrix.JsonPolymorphism
------------------------------------------------------------------------
Declares discriminator-driven polymorphic deserialization on a base
class or interface:

  [JsonConverter(typeof(FallbackTypeConverterFactory))]
  [JsonDiscriminator("type")]
  [JsonKnownType(typeof(ParagraphBlock), "paragraph")]
  [JsonKnownType(typeof(HeadingOneBlock), "heading_1")]
  [JsonFallbackType(typeof(UnsupportedBlock))]
  public interface IBlock { ... }

Types:
  JsonDiscriminatorAttribute(string propertyName)
      names the JSON property whose value selects the concrete type
  JsonKnownTypeAttribute(Type knownType, string discriminatorValue)
      maps one discriminator value to one concrete type (AllowMultiple)
  JsonFallbackTypeAttribute(Type fallbackType)
      catch-all when the discriminator is missing/unknown; without it an
      unmatched discriminator throws JsonException
  FallbackTypeConverter<T>
      the JsonConverter that dispatches reads and serializes writes via
      the value's runtime type
  FallbackTypeConverterFactory
      creates FallbackTypeConverter<T> for every type that declares
      [JsonDiscriminator]; apply via [JsonConverter(...)] on the base
      type, or add one instance to JsonSerializerOptions.Converters

Rules enforced at runtime (InvalidOperationException):
  * known/fallback targets must be assignable to the base type
  * a type may not declare ITSELF as a known/fallback target (declare a
    derived "UnknownXyz" class instead — see UnknownRichText,
    UnknownPropertyValue etc. in CodeBrix.NotionApi)
  * targets must be instantiable, or themselves declare [JsonDiscriminator]
    (two-level dispatch chains are supported)
  * duplicate discriminator values are rejected

NAMING CONSTRAINT: no public type in CodeBrix.JsonPolymorphism may share
a simple name with any public type in System.Text.Json,
System.Text.Json.Serialization, or ...Serialization.Metadata, and none
may start with "JsonPolymorphic" or "JsonDerived". Consumers always have
those STJ usings next to ours; a collision would force using-aliases
(CS0104), which is unacceptable. A verification compile that imports
both namespaces and references every public type by simple name must
stay CS0104-clean when the public surface changes.

CODING CONVENTIONS (CodeBrix family)
------------------------------------------------------------------------
  * TargetFramework net10.0 only; no multi-targeting
  * Nullable reference types OFF — no "string?"/"MyClass?" annotations
    and no null-forgiveness "!" (value-type "int?" etc. are fine)
  * No <ImplicitUsings>, no global usings, no <NoWarn>, no
    warning-suppression properties; warnings are fixed at source
  * File-scoped namespaces only; usings above the namespace line,
    System.* first, alphabetical within groups
  * Ported files carry "//was previously: <upstream namespace>;" on the
    namespace line; new-in-family files do not
  * SITUATIONAL EXCEPTION (AssemblyTools-style): CodeBrix.NotionApi.csproj
    sets GenerateDocumentationFile=false because the upstream
    Notion.Net 5.0.0 surface (500+ types) ships without XML doc comments.
    CodeBrix.JsonPolymorphism has full XML docs and ships its doc file.
  * Tests: xUnit v3 + SilverAssertions + coverlet.collector;
    InternalsVisibleTo grants each library's internals to its .Tests
    project; cancellable calls in tests pass
    TestContext.Current.CancellationToken

ARCHITECTURE
------------------------------------------------------------------------
src/CodeBrix.NotionApi/          (flat namespace CodeBrix.NotionApi)
  Api/            one folder per endpoint group (Blocks, Pages,
                  Databases, DataSources, Users, Search, Comments,
                  FileUploads, Authentication) with Request/Response
                  types beside each client
  Models/         the Notion object model: Blocks, PropertyValue,
                  Database, DataSource, File, Parents, User, Filters,
                  RichText, PropertyItems, FileUpload
  RestClient/     RestClient/IRestClient, ClientOptions, LoggingHandler
  Resilience/     IRetryPolicy, DefaultRetryPolicy, RetryHandler
  Serialization/  ExtensibleEnumConverter<T> (open string-enum structs
                  like BlockType), RuntimeTypeConverterFactory
  Logging/        NotionClientLogging (Microsoft.Extensions.Logging)
  DI/             ServiceCollectionExtensions.AddNotionClient
  Extensions/     EnumExtensions.GetEnumMemberValue (reads
                  [JsonStringEnumMemberName]), HttpResponseMessageExtensions
  Http/           QueryHelpers/HeaderHelpers

src/CodeBrix.JsonPolymorphism/   (namespace CodeBrix.JsonPolymorphism)
  root:      the three attributes + FallbackTypeConverter(+Factory)
  Internal/  DiscriminatorMap (cached reflection over the attributes)

Porting notes (Newtonsoft -> System.Text.Json):
  * [JsonProperty("x")] became [JsonPropertyName("x")]; [EnumMember]
    became [JsonStringEnumMemberName]; StringEnumConverter became
    JsonStringEnumConverter; IsoDateTimeConverter was dropped (STJ
    default is ISO 8601)
  * Newtonsoft honors [JsonProperty] on INTERFACE members as a fallback;
    STJ does not — those attributes were propagated onto the
    implementing classes during the port (218 insertions)
  * Newtonsoft serializes runtime types; STJ serializes declared types —
    RuntimeTypeConverterFactory (internal, registered in
    RestClient.DefaultSerializerOptions) restores runtime-type writing
    for abstract/interface types without their own converter
  * upstream self-referencing JsonSubTypes fallbacks became Unknown*
    subclasses (UnknownRichText, UnknownPropertyValue, UnknownProperty,
    UnknownPropertyItem, UnknownFileObject, UnknownFileObjectWithName,
    UnknownFileImportResult, UnknownDataSourcePropertyConfig)

TESTING
------------------------------------------------------------------------
  dotnet test CodeBrix.NotionApi.slnx

tests/CodeBrix.JsonPolymorphism.Tests — converter/attribute unit tests.
tests/CodeBrix.NotionApi.Tests — ported upstream unit tests. WireMock.Net
was replaced by the in-repo FakeServer harness (same fluent surface:
Given/Request.Create()/Response.Create()/scenario states/LogEntries) and
Moq by the in-repo RecordingRestClient fake. JSON response fixtures live
under tests/CodeBrix.NotionApi.Tests/data/.

tests/CodeBrix.NotionApi.Tests/Integration — ported upstream integration
tests hitting the REAL Notion API. They are opt-in and skip unless these
environment variables are set:
  NOTION_AUTH_TOKEN            an integration token (ntn_...)
  NOTION_PARENT_PAGE_ID        a page the integration can write under
  NOTION_PARENT_DATABASE_ID    a database the integration can write under
These tests CREATE real content in the target workspace.
