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
(pages, blocks, databases, data sources, views, users, comments, custom emojis,
search, file uploads, OAuth), the full Notion object model, and a small set of
CodeBrix-authored authoring helpers on top.

Discriminator-based polymorphic JSON deserialization with fallback types is NOT
built here. It comes from the external CodeBrix.Json.Extensions.MitLicenseForever
package (namespace CodeBrix.Json.Extensions.Polymorphism). The former in-repo
CodeBrix.JsonPolymorphism library and its .Tests project were removed; leftover
bin/obj folders for them may still exist on disk in a stale working copy and
are not tracked.

REPOSITORY LAYOUT
=================
    CodeBrix.NotionApi.slnx          the solution. Its Solution Items folder
                                     carries .gitignore, AGENT-README.txt,
                                     EXTRAS-README.txt, global.json,
                                     icon-codebrix-128.png, LICENSE,
                                     MAINTAINER-README.txt, README-INDEX.txt,
                                     README.md and THIRD-PARTY-NOTICES.txt;
                                     the Tests folder carries the test
                                     project; the library project sits at the
                                     solution root
    global.json                      selects the Microsoft.Testing.Platform
                                     test runner. It does NOT pin an SDK
                                     version. See BUILDING and TESTING below
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
                    Comments, Databases, DataSources, Emojis, FileUploads,
                    Pages, Search, Users, Views), each with Request/ and
                    Response/ types beside its client; ApiEndpoints.cs holds
                    the URL builders
    Models/         the Notion object model: Blocks (+ Blocks/Request),
                    Comment, Common, Database (+ Properties, RichText),
                    DataSource (+ PropertyConfig, Request), File, FileUpload,
                    Filters, Page (+ PageIcon, PageCover), Parents,
                    PropertyItems, PropertyValue, Request, User, View
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

global.json at the repo root does NOT pin an SDK version, so the newest
installed .NET 10 SDK is used. It exists solely to select the test runner —
see TESTING.

`GenerateDocumentationFile` is set to FALSE for this project. This is a
deliberate, situational exception (the AssemblyTools precedent): the ported
upstream surface is many hundreds of public types with no upstream XML doc
comments, and retrofitting summaries onto every member was out of scope. Do NOT
turn the documentation file back on without doing that work — it would produce
a wall of CS1591 warnings. CodeBrix-authored files (Authoring/, DI/, Resilience/,
Serialization/, the factory and exception types) DO carry XML doc comments;
keep it that way for anything new you write.

TESTING
=======
    dotnet test CodeBrix.NotionApi.slnx

THE TEST RUNNER IS Microsoft.Testing.Platform (MTP), selected by global.json at
the repo root:

    { "test": { "runner": "Microsoft.Testing.Platform" } }

Because that setting lives in global.json rather than in the test csproj, it
applies to every `dotnet test` run anywhere in the repository, including CI.
Keep the file committed — without it `dotnet test` falls back to the older
VSTest bridge. You can tell which one ran: MTP output ends in a "Test run
summary:" block, while the VSTest bridge invokes MSBuild with
`--target:VSTest`.

Test project: tests/CodeBrix.NotionApi.Tests (xunit.v3 +
xunit.runner.visualstudio + Microsoft.NET.Test.Sdk + SilverAssertions). There is
no coverlet.collector reference and no coverage collection configured in this
repository. The project has a ProjectReference to the library and copies
tests/CodeBrix.NotionApi.Tests/data/**/*.json to the output directory.

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

AuthenticationClientTests additionally requires, and skips without:

    NOTION_CLIENT_ID            an OAuth application's client id
    NOTION_CLIENT_SECRET        that application's client secret
    NOTION_OAUTH_CODE           a FRESH authorization code (single-use and
                                short-lived, so it cannot be checked in)

NEVER commit a token value into this repository or into any documentation
file. Supply the variables from the environment at run time only. These tests
CREATE REAL CONTENT in the target workspace — point them at a scratch page.

The integration classes all write under the SAME parent page, so they are in a
single non-parallel xUnit collection (Integration/NotionIntegrationCollection.cs,
DisableParallelization = true). Running them in parallel makes Notion return
sporadic HTTP 409 "Conflict occurred while saving". Put any NEW integration test
class in that collection too.

KNOWN COVERAGE GAPS
-------------------
As of the 2026-08-30 re-sync the full suite is 321 tests, 0 failed, verified
BOTH offline and against the real Notion API. These areas are nevertheless
UNVERIFIED against live Notion — they are not known defects, but nothing has
exercised them, so treat them as unproven when you change the code beneath
them:

  * THE OAUTH ENDPOINTS. IAuthenticationClient (CreateTokenAsync,
    RevokeTokenAsync, IntrospectTokenAsync, RefreshTokenAsync) has NEVER run
    against the live API in this repository. Doing so needs a real Notion OAuth
    application plus a freshly minted, single-use authorization code, which is
    why the three tests are env-var gated. This is the largest genuinely
    unverified surface in the library.
  * QueryMeetingNotesAsync RESPONSE SHAPE. The call reaches the correct
    endpoint live — a workspace without the feature answers "This endpoint
    requires a plan with AI meeting notes enabled", which proves the URL, verb
    and auth are right — but no REAL meeting_notes payload has ever been
    deserialized here. MeetingNotesBlock and its four data classes are covered
    only by the synthetic JSON in NewBlockTypeTests /
    ApiVersion20260311ModelTests. Verifying it needs a Notion plan with AI
    meeting notes enabled.
  * VIEW CONFIGURATION IS AN UNTYPED BAG. Every ViewConfiguration subclass
    keeps its per-view-type settings in IDictionary<string, object>
    AdditionalData, and CreateViewRequest.Filter / UpdateViewRequest.Filter /
    View.Filter are plain `object`. That mirrors upstream — Notion has not
    published those shapes — so it is a modelling gap, not a defect, and it is
    documented for consumers in AGENT-README.txt. It does mean a view's
    configuration round-trips without any compile-time checking; if Notion
    publishes the shapes, model them then.

Everything else in the library has been exercised end-to-end against a live
workspace. See ~/ClaudeHome/notion-sdk-net-resync-COMPLETE-2026-08-30.txt (not
in this repo) for the full re-sync record.

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

UPSTREAM RE-SYNC, 2026-08-30
----------------------------
Re-synced against upstream branch main at commit
6e82f3016dc49a5064056a63f4bc4e2b5ffa89c4 (2026-06-17) — upstream's untagged
work toward their 6.0.0. Everything in 5.0.0..main that touches
Src/Notion.Client was brought across:

  * DefaultNotionVersion 2025-09-03 -> 2026-03-11, with the model changes that
    version implies: `archived` deprecated in favour of `in_trash` (DataSource,
    FileUpload, the data-source query/update requests), `transcription` renamed
    to `meeting_notes`, and BlockAppendChildrenRequest.After replaced by
    Position (ContentPosition / AfterBlockContentPosition /
    StartContentPosition / EndContentPosition).
  * New endpoint groups: Views (8 endpoints, ViewsClient) and custom emojis
    (EmojisClient). Both widen the NotionClient constructor and INotionClient.
  * New endpoints on existing clients: Comments RetrieveSingle / Update /
    Delete, Blocks QueryMeetingNotes.
  * New block types: HeadingFour (all three families plus BlockType.Heading4
    and the IBlock registration), Tab, MeetingNotes.
  * New/changed model fields: Page.IsLocked, FileUpload.InTrash,
    ColumnBlock(.Request).WidthRatio, NumberedListItemBlock list_start_index /
    list_format (NumberedListFormat), ParagraphBlock(.Request).Icon,
    PlacePropertyItem, ObjectType.View, VerificationStatus.Unverified,
    VerificationPropertyValue.Info.State typed as VerificationStatus, typed
    StatusConfig / StatusConfigRequest replacing Dictionary<string, object>,
    DateFilter comparison arguments typed as RelativeDateValue.
  * IRestClient/RestClient gained DeleteAsync<T>.
  * The upstream fix to DateCustomConverter that preserves an explicit
    timezone offset instead of flattening it to UTC.

Upstream's duplicate NativeIconObject registration fix required nothing here:
the original port had already landed on the post-fix shape (IconPageIcon
registered for PageIconTypes.Icon, no NativeIcon type at all).

DEFECTS FOUND AND FIXED DURING THE RE-SYNC
------------------------------------------
Four pre-existing defects in this port, none of them upstream's:

  1. RichTextBaseInput was a CONCRETE empty base class. System.Text.Json
     serializes the DECLARED type and does NOT inherit a base class's
     [JsonConverter], so every IEnumerable<RichTextBaseInput> member -- all
     ...UpdateBlock rich text, database and data-source titles, comment bodies
     -- serialized as RichTextBase and silently DROPPED the text/equation/
     mention payload. Fixed by making it abstract, which puts it on
     RuntimeTypeConverterFactory's runtime-type path (the same treatment Filter
     already had).
  2. DataSourcePropertyConfigRequest had the same problem: as a concrete base
     it made CreateDataSourceRequest.Properties emit only the base members,
     dropping "title": {} / "select": {...} and so on. Also made abstract. The
     UPDATE path was already safe via UpdatePropertyConfigurationRequestConverter.
  3. CreateCommentRequest carried no [JsonPropertyName] of its own, so the POST
     body went out as "richText"/"discussionId" and Notion rejected EVERY
     Comments.CreateAsync call with "body.rich_text should be defined, instead
     was `undefined`". The interface-declared names are now repeated on the
     class, as the port convention requires. BlockRetrieveChildrenRequest,
     RetrievePageAsMarkdownRequest and ListEmojisRequest had the same omission
     but were harmless (their values are read as query/path parameters, never
     serialized); they were fixed for consistency.
  4. RestClient built the multipart file content type with
     `new MediaTypeHeaderValue(...)`, which throws FormatException on a content
     type that carries parameters (Notion returns "text/plain; charset=utf-8"
     for a .txt upload). Now uses MediaTypeHeaderValue.Parse.

To guard 1-3, DeclaredTypeSerializationTests.cs asserts the wire shape, and
tests/.../probe-style reflection sweeps were used to confirm no other declared-
type or interface-attribute holes remain.

Test-harness rot fixed at the same time (all pre-existing):
  * The integration tests carried the upstream author's own workspace ids
    (DataSourcesClientTests, BlocksClientTests) and three spent OAuth
    authorization codes (AuthenticationClientTests). Those now come from
    NOTION_PARENT_PAGE_ID and a new NOTION_OAUTH_CODE, or create their own
    resources.
  * tests/.../Integration/assets/notion-logo.png was never copied to the test
    output directory, so both file-upload flow tests failed with
    DirectoryNotFoundException. The tests csproj now copies it.
  * The multi-part upload test split a 4.56 KiB asset, which Notion always
    rejects (every part but the last must be at least 5 MiB). It now builds a
    12 MiB payload in memory.
  * FileUploadsClientTests used an Unsplash URL with an expiring token; it now
    uses the stable Wikimedia file the block tests already reference.
  * The integration classes all wrote under the same parent page in parallel,
    which made Notion return sporadic HTTP 409 "Conflict occurred while
    saving". They are now one non-parallel xUnit collection
    (NotionIntegrationCollection).

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
  * The empty concrete base classes Filter, RichTextBaseInput and
    DataSourcePropertyConfigRequest were made abstract so the runtime-type
    serialization path applies to them. Leaving them concrete silently dropped
    every subtype payload they carried (see "DEFECTS FOUND AND FIXED" above).
  * ACCEPTED DIVERGENCE — JSON string escaping. System.Text.Json's default
    JavaScriptEncoder escapes "+", "&", "<", ">", "'" and every non-ASCII
    character inside string values as \uXXXX, so this library puts
    "2042-11-29T10:30:00\u002B05:00" on the wire where upstream (Newtonsoft)
    writes a literal "+", and escapes accented letters and emoji the same way.
    That is still VALID JSON, Notion decodes it back to the original characters,
    and it has been this library's behaviour since the original port — it only
    became conspicuous once DateFilter started carrying string-backed
    RelativeDateValue values instead of DateTime.
    THIS IS DELIBERATE AND APPROVED: byte-for-byte parity with upstream is not
    a goal here; producing JSON that Notion accepts and understands is. Do NOT
    "fix" it by setting Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping on
    RestClient.DefaultSerializerOptions — that would change the wire format for
    EVERY string the library sends, library-wide, to buy nothing Notion cares
    about. FilterTests.DateFilterTest_WithDateTimeOffset asserts the escaped
    form, and DateFilterTest_WithDateTimeOffset_RoundTripsThroughJson asserts
    that parsing it back yields the literal offset a Notion server sees; keep
    both if you touch this area.
    Number formatting is the same story and equally accepted: System.Text.Json
    writes a double in its shortest round-trippable form, so a whole number goes
    out as -54 where Newtonsoft wrote -54.0. Both were sent to the live API
    against a number property and both returned HTTP 200 with identical results
    (JSON has one number type). FilterTests.NumberFilterTest -- which upstream
    shipped DISABLED with the note "Not sure if integer should be serialized as
    a number with decimals" -- is now enabled and pins the System.Text.Json
    form; NumberFilterTest_WithFractionalValue covers the decimal case.
  * Open string-enum structs (BlockType, Color, ObjectType, PropertyValueType,
    PropertyType, RichTextType, NotionAPIErrorCode, VerificationStatus,
    ViewType, NumberedListFormat, RelativeDateValue) use
    ExtensibleEnumConverter<T> so unknown values from Notion round-trip instead
    of throwing. Each exposes const string ...Value members for attribute use
    and static readonly fields for code; keep both in sync when adding a value.

The default Notion API version the client sends is the DefaultNotionVersion
constant in Constants.cs (currently 2026-03-11). Changing it changes request and response shapes
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
