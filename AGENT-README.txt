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
Entry point (non-DI). NotionClientFactory implements INotionClientFactory;
use the shared NotionClientFactory.Instance, and DISPOSE the client you get
(INotionClient is IDisposable) — a `using` is the simplest way:

  using var client = NotionClientFactory.Instance.Create(new ClientOptions
  {
      AuthToken = "ntn_your_integration_token",
      // BaseUrl, NotionVersion, RetryPolicy, HttpClient are optional
  });

INotionClient (and the RestClient it wraps) is IDisposable. Disposing it
releases ONLY an HttpClient the library created and owns internally; a client
you supplied via ClientOptions.HttpClient, or one drawn from an
IHttpClientFactory, is left for its owner to manage.

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

DEPENDENCY INJECTION — the correct setup
------------------------------------------------------------------------
THE RECOMMENDED registration is AddNotionClientFactory: it registers
INotionClientFactory as a singleton backed by an IHttpClientFactory-managed
named HttpClient. Inject INotionClientFactory, then create (and dispose) an
INotionClient per unit of work — the HttpClient is drawn from the pooled,
rotated IHttpClientFactory, so per-scope creation is cheap and safe:

  // Startup / registration:
  services.AddNotionClientFactory();

  // Consuming class — inject INotionClientFactory:
  public sealed class MyNotionWorker(INotionClientFactory notionFactory)
  {
      public async Task DoWorkAsync(string authToken)
      {
          using var notion = notionFactory.Create(new ClientOptions
          {
              AuthToken = authToken,
          });

          var me = await notion.Users.MeAsync();
          // ... use `notion` for as many calls as this unit of work needs ...
      }   // `notion` is disposed here; the pooled HttpClient is left alone.
  }

Every INotionClient/RestClient the factory creates obtains its HttpClient
from the one DI-configured IHttpClientFactory. Because it is resolved PER
REQUEST, handler rotation (stale-DNS / socket-exhaustion protection) is
preserved — never cache a single CreateClient() result for the app lifetime.
When you control construction directly, prefer the
NotionClientFactory(IHttpClientFactory) constructor; SetHttpClientFactory
exists for configuring the shared NotionClientFactory.Instance and is a
set-once, thread-safe operation.

Alternative — services.AddNotionClient(options => { ... }) registers a
single app-wide INotionClient as a singleton (also IHttpClientFactory-backed
and resolved per request). Use it when one shared client is all you need and
you do not want to create clients per unit of work; the container disposes it
at shutdown, so do NOT dispose an injected INotionClient yourself here.

DO NOT create a NotionClient per request WITHOUT an IHttpClientFactory (no
ClientOptions.HttpClient and no factory): each one builds and owns a fresh
HttpClient + connection pool, and creating many of them leaks sockets. Either
reuse one long-lived client, or back creation with an IHttpClientFactory as
shown above.

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

AUTHORING HELPERS (start here for building pages)
------------------------------------------------------------------------
For the common "build a page from local content" workflow, prefer the
authoring helpers in the CodeBrix.NotionApi namespace over hand-writing
request objects. They smooth over Notion's hard limits and the raw SDK's
nested-`.Info` ceremony, and they are the recommended path. Three types:

  NotionText   — rich-text runs.
    NotionText.Run(text, bold:, italic:, code:, linkUrl:)  -> one run
    NotionText.Split(text, ...)      -> runs, auto-split at the 2,000-char
                                        per-run limit on WORD boundaries
    NotionText.Plain(text) / PlainBase(text)  -> unannotated run list
    (Notion REJECTS any rich-text run > 2000 chars; Split handles it.)

  NotionBlocks — terse block-request factories (no nested `.Info`):
    Paragraph, Heading2, Heading3, Callout(text, emoji), Quote, Bullet,
    Numbered, Toggle(summary, children), Divider, Table(rows, header),
    TableRow, ImageExternal(url), ImageUpload(fileUploadId).
    The string overloads treat their argument as LITERAL text (no markdown
    is parsed) — build emphasis explicitly with NotionText.Run.

  NotionAuthoringExtensions — INotionClient extension methods:
    CreateChildPageAsync(parentPageId, title)   create a titled sub-page
    AppendChildrenBatchedAsync(parentId, blocks, throttleMs:)
        appends ANY number of blocks, auto-splitting into requests of <=100
        (Notion's per-request cap) in order, with optional rate-limit pacing
    UploadFileAsync(filePath, contentType:)   single-part file upload
        (Create+Send in one call) -> returns the file_upload id for
        NotionBlocks.ImageUpload; content type inferred from the extension
    ArchivePageAsync(pageId)      trash a page (in_trash = true)
    RetrieveAllChildrenAsync(blockId)   read every child block, following
        pagination to the end
    GuessContentType(path)        MIME type from a file extension

Blessed end-to-end pattern (create with a title, then append the body):

  var page = await client.CreateChildPageAsync(parentPageId, "My Page");

  var uploadId = await client.UploadFileAsync("/path/diagram.png");
  var blocks = new List<IBlockObjectRequest>
  {
      NotionBlocks.Heading2("Introduction"),
      NotionBlocks.Paragraph("Body text ..."),
      NotionBlocks.Paragraph(new List<RichTextBase>
      {
          NotionText.Run("A "), NotionText.Run("bold", bold: true),
          NotionText.Run(" word."),
      }),
      NotionBlocks.ImageUpload(uploadId, NotionText.PlainBase("A caption")),
      NotionBlocks.Callout("Heads up!", "\U0001F680"),
  };
  await client.AppendChildrenBatchedAsync(page.Id, blocks);

For whole-document MARKDOWN (rather than block-by-block construction),
Notion has native Notion-flavored-markdown ingestion — see
PagesCreateParameters.Markdown / PagesCreateParametersBuilder.SetMarkdown,
Pages.UpdateMarkdownAsync, and Pages.RetrieveAsMarkdownAsync — which can
be simpler than building blocks when you don't need fine interleaving.

PAGE & BLOCK ORDERING (positional control) — a Notion gotcha
------------------------------------------------------------------------
Notion keeps a page's children (sub-pages AND content blocks) in
INSERTION order, and its API offers NO operation to reposition an
existing child in place. This is a Notion API constraint, not a
limitation of this library — the library models the endpoints faithfully.

What you CANNOT do:
  * Reorder existing sibling pages/blocks. There is no "reorder" or
    "move block" endpoint.
  * Reposition with Move. Pages.MoveAsync (POST /v1/pages/{id}/move,
    MovePageRequest) accepts ONLY a new `parent` (MovePageParent) — the
    Notion endpoint has no `position` parameter. It re-parents a page; it
    cannot change a page's order under the same parent. (MovePageBody-
    Parameters exposes only Parent for exactly this reason.)

What you CAN do — positional control exists only at INSERT time:
  * Create a page at a chosen slot. Set PagesCreateParameters.Position to
    a PagePosition subtype:
        new PageStartPosition()                      // first under parent
        new PageEndPosition()                        // last (also the default)
        new AfterBlockPagePosition {                 // right after a sibling
            AfterBlock = new AfterBlockReference { Id = siblingBlockId } }
  * Append blocks at a chosen slot. Notion added a `position` object
    (after_block / start / end) in API version 2026-03-11. This library
    currently pins NotionVersion "2025-09-03", whose append endpoint uses
    the flat BlockAppendChildrenRequest.After (a block id to append
    after); with no After set, appended blocks go to the end.

Consequence / recipe: because there is no in-place reorder, get ordering
right by CREATING in the desired order (optionally via Position). To fix
the order of pages that already exist, archive them
(Pages.UpdateAsync with PagesUpdateParameters.InTrash = true) and
recreate them in sequence; there is no cheaper reorder path.

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
