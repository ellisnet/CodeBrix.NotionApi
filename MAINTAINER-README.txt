================================================================================
MAINTAINER-README: CodeBrix.NotionApi
Notes for people and agents MAINTAINING this repository — not for package
consumers
================================================================================

If you are consuming the NuGet package, read AGENT-README.txt instead. This
file is only about working ON this repository.

PURPOSE AND SCOPE
=================
This repository builds ONE assembly and ships ONE NuGet package:

    assembly    CodeBrix.NotionApi
    namespace   CodeBrix.NotionApi                (single, flat)
    package id  CodeBrix.NotionApi.MitLicenseForever
    covered by  AGENT-README.txt (repo root)

The assembly is a .NET client library for the Notion API: every endpoint group
(pages, blocks, databases, data sources, users, comments, search, file uploads,
OAuth), the full Notion object model, and a small set of CodeBrix-authored
authoring helpers on top.

Discriminator-based polymorphic JSON deserialization with fallback types is NOT
built here. It comes from the external CodeBrix.Json.Extensions.MitLicenseForever
package (namespace CodeBrix.Json.Extensions.Polymorphism). The former in-repo
CodeBrix.JsonPolymorphism library and its .Tests project were removed; leftover
bin/obj folders for them may still exist on disk in a stale working copy and
are not tracked.

REPOSITORY LAYOUT
=================
    CodeBrix.NotionApi.slnx          the solution (Solution Items + Tests
                                     folder + the library project)
    LICENSE                          MIT
    THIRD-PARTY-NOTICES.txt          upstream attribution and the full record
                                     of what was ported and how
    README.md                        human-facing overview (GitHub / nuget.org)
    AGENT-README.txt                 consumer documentation; SHIPS in the nupkg
    MAINTAINER-README.txt            this file
    EXTRAS-README.txt                non-package content in the repo
    README-INDEX.txt                 map of the README files
    icon-codebrix-128.png            package icon
    src/CodeBrix.NotionApi/          the library
    tests/CodeBrix.NotionApi.Tests/  unit tests + opt-in integration tests

Library source folders (all in the flat CodeBrix.NotionApi namespace):

    Api/            one folder per endpoint group (Authentication, Blocks,
                    Comments, Databases, DataSources, FileUploads, Pages,
                    Search, Users), each with Request/ and Response/ types
                    beside its client; ApiEndpoints.cs holds the URL builders
    Models/         the Notion object model: Blocks (+ Blocks/Request),
                    Comment, Common, Database (+ Properties, RichText),
                    DataSource (+ PropertyConfig, Request), File, FileUpload,
                    Filters, Page (+ PageIcon, PageCover), Parents,
                    PropertyItems, PropertyValue, Request, User
    RestClient/     RestClient / IRestClient, ClientOptions, LoggingHandler
    Resilience/     IRetryPolicy, DefaultRetryPolicy, RetryHandler
    Serialization/  ExtensibleEnumConverter<T>, RuntimeTypeConverterFactory
    Logging/        NotionClientLogging, Log
    DI/             ServiceCollectionExtensions
    Extensions/     EnumExtensions, HttpResponseMessageExtensions
    Http/           QueryHelpers
    Authoring/      NotionText, NotionBlocks, NotionAuthoringExtensions
                    (CodeBrix-authored, not from upstream)
    root            NotionClient, NotionClientFactory, INotionClientFactory,
                    Constants, NotionApiException,
                    NotionApiRateLimitException, NotionAPIErrorCode,
                    NotionApiErrorResponse, InternalsVisibleTo

BUILDING
========
    dotnet restore CodeBrix.NotionApi.slnx
    dotnet build   CodeBrix.NotionApi.slnx

The library targets net10.0 only; there is no multi-targeting.

`GenerateDocumentationFile` is set to FALSE for this project. This is a
deliberate, situational exception (the AssemblyTools precedent): the ported
upstream surface is roughly 590 public types with no upstream XML doc comments,
and retrofitting summaries onto every member was out of scope. Do NOT turn the
documentation file back on without doing that work — it would produce a wall of
CS1591 warnings. CodeBrix-authored files (Authoring/, DI/, Resilience/,
Serialization/, the factory and exception types) DO carry XML doc comments;
keep it that way for anything new you write.

TESTING
=======
    dotnet test CodeBrix.NotionApi.slnx

Test project: tests/CodeBrix.NotionApi.Tests (xUnit v3 + xunit.runner.visualstudio
+ Microsoft.NET.Test.Sdk + coverlet.collector + SilverAssertions). It has a
ProjectReference to the library and copies tests/CodeBrix.NotionApi.Tests/data/
**/*.json to the output directory.

Conventions in this test project:
  * Test class files are named <Class>Tests.cs; test method bodies use the
    //Arrange //Act //Assert comment blocks.
  * Cancellable calls in tests pass TestContext.Current.CancellationToken.
  * InternalsVisibleTo.cs grants the library's internals to
    CodeBrix.NotionApi.Tests, which several tests rely on (for example
    FilterTests subclasses RestClient to reach the internal
    DefaultSerializerOptions).
  * WireMock.Net was replaced by the in-repo FakeServer harness, which keeps
    the same fluent surface (Given / Request.Create() / Response.Create() /
    scenario states / LogEntries). Moq was replaced by the in-repo
    RecordingRestClient fake. Both are CodeBrix-authored; do not reintroduce
    WireMock.Net or Moq.
  * JSON response fixtures live under tests/CodeBrix.NotionApi.Tests/data/
    (blocks, databases, pages, search, users).

Integration tests (tests/CodeBrix.NotionApi.Tests/Integration) hit the REAL
Notion API and are OPT-IN. IntegrationTestBase calls Assert.SkipWhen on each of
these environment variables, so the whole class is skipped when they are
absent:

    NOTION_AUTH_TOKEN           a Notion integration token
    NOTION_PARENT_PAGE_ID       a page the integration may write under
    NOTION_PARENT_DATABASE_ID   a database the integration may write under

NEVER commit a token value into this repository or into any documentation
file. Supply the variables from the environment at run time only. These tests
CREATE REAL CONTENT in the target workspace — point them at a scratch page.

PACKAGING AND PUBLISHING
========================
The library project sets GeneratePackageOnBuild=true, so `dotnet build` in
Release already produces the .nupkg; `dotnet pack` also works. There is no
custom pack driver and no TargetsForTfmSpecificBuildOutput hook — it is a plain
one-csproj-one-nupkg build.

Package metadata lives in src/CodeBrix.NotionApi/CodeBrix.NotionApi.csproj:
PackageId CodeBrix.NotionApi.MitLicenseForever, PackageLicenseExpression MIT,
PackageRequireLicenseAcceptance true, PackageIcon icon-codebrix-128.png,
PackageReadmeFile README.md, project and repository URLs pointing at
https://github.com/ellisnet/CodeBrix.NotionApi.

Files packed alongside the assembly (all from the repo root):
    icon-codebrix-128.png
    README.md
    AGENT-README.txt          <- the consumer guide ships in the package
    THIRD-PARTY-NOTICES.txt
MAINTAINER-README.txt, EXTRAS-README.txt and README-INDEX.txt are NOT packed.
If you add another AGENT-README file, add it to that ItemGroup too.

Versioning is the CodeBrix date-stamped scheme, computed in the csproj from
System.DateTime.UtcNow: major is pinned to 1, minor is whole years since
_VersionBaseYear, build is the UTC day of year (1-based), revision is the UTC
minute of day (0-1439). The value is strictly increasing over time and is NOT
SemVer — major/minor say nothing about API compatibility. Consequences:
  * every build produces a new version and, with GeneratePackageOnBuild, a
    fresh .nupkg;
  * two builds inside the same UTC minute produce the SAME version, so do not
    publish twice within one minute;
  * to re-baseline the minor number, change _VersionBaseYear in the csproj.

Tag the repository to match the published nuget.org version when you publish.

PROVENANCE AND VENDORED SOURCES
===============================
The production source under src/CodeBrix.NotionApi/ was ported from the
Src/Notion.Client tree of notion-sdk-net 5.0.0 (tag 5.0.0, commit
bfe6e6a2c8a079f2394cbb06f7ef9eafec260d76), MIT licensed, Copyright (c) 2021
Vedant Koditkar. The tests were ported from that release's Test/Notion.UnitTests
and Test/Notion.IntegrationTests trees, including the JSON fixtures and the
notion-logo.png upload asset. THIRD-PARTY-NOTICES.txt is authoritative and must
stay in sync with any further porting work.

Files that are CodeBrix-authored rather than ported:
    src/CodeBrix.NotionApi/InternalsVisibleTo.cs
    src/CodeBrix.NotionApi/Serialization/RuntimeTypeConverterFactory.cs
    the eight Unknown* fallback classes (UnknownRichText, UnknownPropertyValue,
        UnknownProperty, UnknownPropertyItem, UnknownFileObject,
        UnknownFileObjectWithName, UnknownFileImportResult,
        UnknownDataSourcePropertyConfig)
    src/CodeBrix.NotionApi/Authoring/* (NotionText, NotionBlocks,
        NotionAuthoringExtensions)
    tests: FakeServer.cs, RecordingRestClient.cs, ApiTestBase.cs,
        Integration/IntegrationTestBase.cs

Every ported .cs file carries "//was previously: <upstream namespace>;" on its
namespace line; CodeBrix-authored files do not. Preserve that marker when you
edit a ported file, and add it to any file you newly port.

Port decisions worth knowing before you change serialization:
  * [JsonProperty("x")] became [JsonPropertyName("x")]; [EnumMember] became
    [JsonStringEnumMemberName]; StringEnumConverter became
    JsonStringEnumConverter; IsoDateTimeConverter was dropped because
    System.Text.Json already defaults to ISO 8601.
  * Newtonsoft.Json honours [JsonProperty] declared on INTERFACE members as a
    fallback; System.Text.Json does not, so those attributes were propagated
    onto the implementing classes during the port.
  * Newtonsoft.Json serializes the RUNTIME type; System.Text.Json serializes
    the DECLARED type. RuntimeTypeConverterFactory (internal, registered in
    RestClient.DefaultSerializerOptions) restores runtime-type writing for
    abstract/interface-typed members that have no converter of their own. Many
    request models depend on it — do not remove it, and do not serialize these
    models with bare JsonSerializerOptions.
  * Upstream JsonSubTypes fallback declarations that pointed at the declaring
    type itself became the Unknown* subclasses, because the converter cannot
    dispatch a type to itself without recursing.
  * The empty concrete base class Filter was made abstract so the
    runtime-type serialization path applies to it.
  * Open string-enum structs (BlockType, Color, ObjectType, PropertyValueType,
    PropertyType, RichTextType, NotionAPIErrorCode, VerificationStatus) use
    ExtensibleEnumConverter<T> so unknown values from Notion round-trip instead
    of throwing. Each exposes const string ...Value members for attribute use
    and static readonly fields for code; keep both in sync when adding a value.

The default Notion API version the client sends is the DefaultNotionVersion
constant in Constants.cs. Changing it changes request and response shapes
across the whole model layer — treat it as a breaking change and update the
models and AGENT-README together.

CODING CONVENTIONS
==================
CodeBrix family rules that apply here:
  * TargetFramework net10.0 only; no multi-targeting.
  * Nullable reference types OFF — no "string?" / "MyClass?" annotations and no
    null-forgiveness "!" (nullable VALUE types such as int? / bool? are fine
    and are used heavily in this model layer).
  * No <ImplicitUsings>, no global usings, no <NoWarn>, no warning-suppression
    properties; warnings are fixed at source.
  * File-scoped namespaces only; usings above the namespace line, System.*
    first, alphabetical within groups.
  * Situational exception, already applied: GenerateDocumentationFile=false
    (see BUILDING).
  * The only upstream this library is derived from is notion-sdk-net, and
    THIRD-PARTY-NOTICES.txt is the record of that. If you port anything else
    in, add it there in the same shape before you commit the code.

NOTES
=====
  * The library has no public API for deleting a page; that is a Notion API
    fact (pages are trashed via in_trash), not an omission to fix.
  * IBlocksClient.UpdateAsync takes IUpdateBlock, a family separate from both
    IBlock and IBlockObjectRequest. When adding support for a new Notion block
    type, add all three (response Block, ...BlockRequest, ...UpdateBlock) plus
    the BlockType constant and the [JsonKnownType] entry on IBlock.
  * A few ...UpdateBlock types (BookmarkUpdateBlock, BreadcrumbUpdateBlock,
    DividerUpdateBlock, TableOfContentsUpdateBlock) implement IUpdateBlock
    directly instead of deriving from UpdateBlock. That is upstream's shape;
    changing it is a behaviour change for InTrash.
  * The file
    Api/Blocks/RequestParams/BlocksUpdateParameters/UpdateBlocks/HeadingThreeeUpdateBlock.cs
    has a misspelled FILE name (three e's); the class inside it is correctly
    named HeadingThreeUpdateBlock. Renaming the file is safe but is a
    cosmetic-only change.
  * ServiceCollectionExtensions lives in the CodeBrix.NotionApi namespace, not
    Microsoft.Extensions.DependencyInjection, even though its namespace line
    records the upstream location. Consumers therefore need
    `using CodeBrix.NotionApi;` in startup code.
