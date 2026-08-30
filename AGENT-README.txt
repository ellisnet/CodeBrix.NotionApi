================================================================================
AGENT-README: CodeBrix.NotionApi
A Guide for AI Coding Agents — CONSUMING the CodeBrix.NotionApi.MitLicenseForever
NuGet package
================================================================================

OVERVIEW
========
CodeBrix.NotionApi is a fully managed .NET client library for the Notion API.
It covers every Notion endpoint group — pages, blocks, databases, data sources,
views, users, comments, custom emojis, search, file uploads and OAuth — plus the
complete Notion
object model (rich text, property values, property items, block types, parents,
icons, covers, files) and a set of high-level authoring helpers.

Target framework: .NET 10 or later.

Provenance: the library is a port of notion-sdk-net (the Notion.Net package),
rebuilt on System.Text.Json with no Newtonsoft.Json and no
JsonSubTypes dependency. The upstream `Notion.Client` namespace became the
single flat namespace `CodeBrix.NotionApi`; type names are otherwise unchanged.
DO NOT reference the upstream package or write `using Notion.Client;` — the
upstream namespaces do not exist in this assembly. Full attribution is in
THIRD-PARTY-NOTICES.txt, which ships in the NuGet package.

The client sends the Notion API version header `Notion-Version: 2026-03-11`
unless you override `ClientOptions.NotionVersion`. That version string is a
protocol fact, not a package version: it decides which request/response shapes
Notion applies, and the models in this library are shaped for it.

WHAT 2026-03-11 CHANGED (read this if you are upgrading)
--------------------------------------------------------
  * `archived` is deprecated everywhere in favour of `in_trash`. The `Archived`
    properties are still present and still serialize, but are marked
    [Obsolete]: use `InTrash`. Notion no longer returns `archived` on a page.
  * The `transcription` block type was renamed `meeting_notes`. `TranscriptionBlock`
    and `BlockType.Transcription` are [Obsolete] but still deserialize, so
    payloads from a workspace pinned to an older version keep working. New code
    reads `MeetingNotesBlock`.
  * `BlockAppendChildrenRequest.After` (a block id string) was REPLACED by
    `Position`, a `ContentPosition` object. This is a compile-time break; see
    "Appending, updating and deleting blocks".
  * New block types: `heading_4` and `tab`.
  * New endpoint groups: Views (`notion.Views`) and custom emojis
    (`notion.Emojis`); new methods on Comments and Blocks.
  * `DateFilter`'s comparison arguments moved from `DateTime?` to
    `RelativeDateValue?`, which also accepts relative keywords like "today".
    Implicit conversions from `DateTime`, `DateTimeOffset` and `string` mean
    most existing call sites still compile unchanged.
  * `StatusProperty.Status` and `StatusDataSourcePropertyConfig.Status` are now
    a typed `StatusConfig` instead of `Dictionary<string, object>`.
  * `VerificationPropertyValue.Info.State` is now a `VerificationStatus`
    instead of a `string`, and gained the `Unverified` value.
  * `Page.IsLocked` (`bool?`) and `FileUpload.InTrash` were added.

INSTALLATION
============
NuGet package id:  CodeBrix.NotionApi.MitLicenseForever

    dotnet add package CodeBrix.NotionApi.MitLicenseForever

License: MIT.

The package id carries the ".MitLicenseForever" suffix; the assembly and the
namespace do NOT — code always says `CodeBrix.NotionApi`.

NuGet dependencies (all resolved automatically; no versions given here on
purpose — take whatever the package pulls in):
    CodeBrix.Json.Extensions.MitLicenseForever
        Discriminator-driven polymorphic JSON deserialization with fallback
        types (namespace CodeBrix.Json.Extensions.Polymorphism). See
        "POLYMORPHIC DESERIALIZATION" below and, for its own full API, that
        package's AGENT-README.
    Microsoft.Extensions.Logging.Abstractions
    Microsoft.Extensions.DependencyInjection.Abstractions
    Microsoft.Extensions.Http

No native libraries, no OS restrictions. The only runtime requirement is
outbound HTTPS to https://api.notion.com and a Notion integration token.

KEY NAMESPACES / USINGS
=======================
    using CodeBrix.NotionApi;   // the ENTIRE client and model surface —
                                // one flat namespace, ~590 public types

    using CodeBrix.Json.Extensions.Polymorphism;  // only if you declare your
                                // own [JsonDiscriminator]/[JsonKnownType]/
                                // [JsonFallbackType] hierarchies

`ServiceCollectionExtensions` (AddNotionClient / AddNotionClientFactory) also
lives in `CodeBrix.NotionApi`, NOT in
`Microsoft.Extensions.DependencyInjection` — add the `using CodeBrix.NotionApi;`
line in your startup file to see those extension methods.

CORE API REFERENCE — CLIENT AND SUB-CLIENTS
===========================================
Creating a client (non-DI). `NotionClientFactory` implements
`INotionClientFactory`; use the shared `NotionClientFactory.Instance` and
DISPOSE the client you get (`INotionClient : IDisposable`):

    using var notion = NotionClientFactory.Instance.Create(new ClientOptions
    {
        AuthToken = "ntn_your_integration_token",
        // BaseUrl, NotionVersion, RetryPolicy, HttpClient are optional
    });

INotionClientFactory
--------------------
    INotionClient Create(ClientOptions options)

NotionClientFactory : INotionClientFactory
------------------------------------------
    NotionClientFactory()
    NotionClientFactory(IHttpClientFactory httpClientFactory)
    static NotionClientFactory Instance { get; }        // process-wide, lazy
    void SetHttpClientFactory(IHttpClientFactory httpClientFactory)
        Set-once, thread-safe. Throws InvalidOperationException if a factory was
        already set or if this instance already created a client.
    INotionClient Create(ClientOptions options)

ClientOptions
-------------
    string      BaseUrl        { get; set; }   // default https://api.notion.com/
    string      NotionVersion  { get; set; }   // default 2026-03-11
    string      AuthToken      { get; set; }   // integration token (Bearer)
    IRetryPolicy RetryPolicy   { get; set; }   // opt-in; null = no retries
    HttpClient  HttpClient     { get; set; }   // caller-owned when supplied

Disposing an INotionClient disposes its IRestClient, which releases ONLY an
HttpClient the library created and owns. An HttpClient you supplied through
ClientOptions.HttpClient, or one drawn from an IHttpClientFactory, is left for
its owner.

INotionClient : IDisposable — one sub-client per endpoint group
--------------------------------------------------------------
    IUsersClient          Users                { get; }
    IPagesClient          Pages                { get; }
    IDatabasesClient      Databases            { get; }
    IDataSourcesClient    DataSources          { get; }
    IViewsClient          Views                { get; }
    IBlocksClient         Blocks               { get; }
    ISearchClient         Search               { get; }
    ICommentsClient       Comments             { get; }
    IEmojisClient         Emojis               { get; }
    IFileUploadsClient    FileUploads          { get; }
    IAuthenticationClient AuthenticationClient { get; }
    IRestClient           RestClient           { get; }

Note the property name is `AuthenticationClient`, not `Authentication`.

EVERY method below takes a trailing
`CancellationToken cancellationToken = default`; it is shown once per interface
and then elided for readability. The lists are COMPLETE — these interfaces have
no other members.

IPagesClient
------------
    Task<Page> CreateAsync(
        PagesCreateParameters pagesCreateParameters,
        CancellationToken cancellationToken = default)
    Task<Page> RetrieveAsync(string pageId, ...)
    Task<Page> UpdatePropertiesAsync(
        string pageId, IDictionary<string, PropertyValue> updatedProperties, ...)
    Task<Page> UpdateAsync(
        string pageId, PagesUpdateParameters pagesUpdateParameters, ...)
    Task<IPropertyItemObject> RetrievePagePropertyItemAsync(
        RetrievePropertyItemParameters retrievePropertyItemParameters, ...)
    Task<PageMarkdownResponse> RetrieveAsMarkdownAsync(
        RetrievePageAsMarkdownRequest request, ...)
    Task<Page> MoveAsync(MovePageRequest request, ...)
    Task<PageMarkdownResponse> UpdateMarkdownAsync(
        string pageId, UpdatePageMarkdownBody body, ...)

There is NO Pages.DeleteAsync — trash a page with
`UpdateAsync(pageId, new PagesUpdateParameters { InTrash = true })`.

IDatabasesClient
----------------
    Task<Database> RetrieveAsync(string databaseId, ...)
    Task<Database> CreateAsync(
        DatabasesCreateRequest databasesCreateParameters, ...)
    Task<Database> UpdateAsync(DatabasesUpdateRequest databasesUpdateRequest, ...)

IMPORTANT: there is NO QueryAsync on IDatabasesClient. Under the 2026-03-11
API a database is a container of DATA SOURCES, and querying rows lives on
`IDataSourcesClient.QueryAsync`. Read the database first
(`Databases.RetrieveAsync`), take a `DataSourceId` out of its `DataSources`
collection, then query that.

IDataSourcesClient
------------------
    Task<DataSource> RetrieveAsync(RetrieveDataSourceRequest request, ...)
    Task<DataSource> CreateAsync(CreateDataSourceRequest request, ...)
    Task<DataSource> UpdateAsync(UpdateDataSourceRequest request, ...)
    Task<ListDataSourceTemplatesResponse> ListDataSourceTemplatesAsync(
        ListDataSourceTemplatesRequest request, ...)
    Task<QueryDataSourceResponse> QueryAsync(QueryDataSourceRequest request, ...)

IBlocksClient
-------------
    Task<IBlock> RetrieveAsync(string blockId, ...)
    Task<IBlock> UpdateAsync(string blockId, IUpdateBlock updateBlock, ...)
    Task<RetrieveChildrenResponse> RetrieveChildrenAsync(
        BlockRetrieveChildrenRequest request, ...)
    Task<AppendChildrenResponse> AppendChildrenAsync(
        BlockAppendChildrenRequest request, ...)
    Task DeleteAsync(string blockId, ...)          // moves the block to trash
    Task<QueryMeetingNotesResponse> QueryMeetingNotesAsync(
        QueryMeetingNotesRequest request, ...)

IViewsClient
------------
    Task<PaginatedList<View>> ListAsync(ListViewsRequest request, ...)
    Task<View> CreateAsync(CreateViewRequest request, ...)
    Task<View> RetrieveAsync(string viewId, ...)
    Task<View> UpdateAsync(UpdateViewRequest request, ...)
    Task<View> DeleteAsync(string viewId, ...)
    Task<ViewQueryResponse> CreateQueryAsync(CreateViewQueryRequest request, ...)
    Task<PaginatedList<PageReference>> GetQueryResultsAsync(
        GetViewQueryResultsRequest request, ...)
    Task<DeletedViewQueryResponse> DeleteQueryAsync(
        DeleteViewQueryRequest request, ...)

IEmojisClient
-------------
    Task<ListEmojisResponse> ListAsync(ListEmojisRequest request, ...)

IUsersClient
------------
    Task<User> RetrieveAsync(string userId, ...)
    Task<ListUsersResponse> ListAsync(
        CancellationToken cancellationToken = default)
    Task<ListUsersResponse> ListAsync(ListUsersRequest listUsersRequest, ...)
    Task<User> MeAsync(CancellationToken cancellationToken = default)

ISearchClient
-------------
    Task<SearchResponse> SearchAsync(SearchRequest request, ...)

ICommentsClient
---------------
    Task<Comment> CreateAsync(CreateCommentRequest createCommentParameters, ...)
    Task<RetrieveCommentsResponse> RetrieveAsync(
        RetrieveCommentsRequest parameters, ...)
    Task<Comment> RetrieveSingleAsync(RetrieveSingleCommentRequest request, ...)
    Task<Comment> UpdateAsync(UpdateCommentRequest request, ...)
    Task DeleteAsync(string commentId, ...)

IFileUploadsClient
------------------
    Task<FileUpload> CreateAsync(
        CreateFileUploadRequest fileUploadObjectRequest, ...)
    Task<FileUpload> SendAsync(SendFileUploadRequest sendFileUploadRequest, ...)
    Task<FileUpload> CompleteAsync(
        CompleteFileUploadRequest completeFileUploadRequest, ...)
    Task<ListFileUploadsResponse> ListAsync(ListFileUploadsRequest request, ...)
    Task<FileUpload> RetrieveAsync(RetrieveFileUploadRequest request, ...)

IAuthenticationClient (OAuth; not needed for a plain integration token)
----------------------------------------------------------------------
    Task<CreateTokenResponse> CreateTokenAsync(
        CreateTokenRequest createTokenRequest, ...)
    Task RevokeTokenAsync(RevokeTokenRequest revokeTokenRequest, ...)
    Task<IntrospectTokenResponse> IntrospectTokenAsync(
        IntrospectTokenRequest introspectTokenRequest, ...)
    Task<RefreshTokenResponse> RefreshTokenAsync(
        RefreshTokenRequest refreshTokenRequest, ...)

IRestClient : IDisposable (escape hatch for endpoints not yet modelled)
----------------------------------------------------------------------
    Task<T> GetAsync<T>(string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null, ...)
    Task<T> PostAsync<T>(string uri, object body,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null, ...)
    Task<T> PostAsync<T>(string uri, ISendFileUploadFormDataParameters formData,
        ... same optional parameters ...)
    Task<T> PatchAsync<T>(string uri, object body,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null, ...)
    Task DeleteAsync(string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null, ...)
    Task<T> DeleteAsync<T>(string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null, ...)

`ApiEndpoints` is a public static class of URL builders
(`ApiEndpoints.PagesApiUrls.Retrieve(pageId)`, `ApiEndpoints.BlocksApiUrls`,
`ApiEndpoints.UsersApiUrls`, `ApiEndpoints.SearchApiUrls`,
`ApiEndpoints.CommentsApiUrls`, `ApiEndpoints.DatabasesApiUrls`,
`ApiEndpoints.FileUploadsApiUrls`, `ApiEndpoints.AuthenticationUrls`,
`ApiEndpoints.DataSourcesApiUrls`, `ApiEndpoints.ViewsApiUrls`,
`ApiEndpoints.EmojisApiUrls`) if you need a path for IRestClient.

DEPENDENCY INJECTION — the correct setup
========================================
THE RECOMMENDED registration is `AddNotionClientFactory`: it registers
`INotionClientFactory` as a singleton backed by an IHttpClientFactory-managed
named HttpClient. Inject `INotionClientFactory`, then create (and dispose) an
`INotionClient` per unit of work — the HttpClient is drawn from the pooled,
rotated IHttpClientFactory, so per-scope creation is cheap and safe:

    // Startup / registration:
    services.AddNotionClientFactory();
    // optional: services.AddNotionClientFactory(http => http.Timeout = ...);

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

Every INotionClient the factory creates obtains its HttpClient from the one
DI-configured IHttpClientFactory. Because it is resolved PER REQUEST, handler
rotation (stale-DNS / socket-exhaustion protection) is preserved — never cache
a single CreateClient() result for the application lifetime. When you control
construction directly, prefer the `NotionClientFactory(IHttpClientFactory)`
constructor; `SetHttpClientFactory` exists for configuring the shared
`NotionClientFactory.Instance` and is a set-once, thread-safe operation.

Alternative — `services.AddNotionClient(options => { ... })` registers a single
app-wide `INotionClient` as a singleton (also IHttpClientFactory-backed and
resolved per request). Use it when one shared client is all you need and you do
not want to create clients per unit of work; the container disposes it at
shutdown, so do NOT dispose an injected INotionClient here.

    services.AddNotionClient(options =>
    {
        options.AuthToken = configuration["Notion:Token"];
        options.RetryPolicy = new DefaultRetryPolicy();
    });

Signatures:
    static IServiceCollection AddNotionClientFactory(
        this IServiceCollection services, Action<HttpClient> configureClient = null)
    static IServiceCollection AddNotionClient(
        this IServiceCollection services, Action<ClientOptions> configureOptions)

DO NOT create a NotionClient per request WITHOUT an IHttpClientFactory (no
ClientOptions.HttpClient and no factory): each one builds and owns a fresh
HttpClient plus connection pool, and creating many of them leaks sockets.
Either reuse one long-lived client, or back creation with an IHttpClientFactory
as shown above.

Logging is opt-in and static:
    NotionClientLogging.ConfigureLogger(ILoggerFactory loggerFactory)

ERRORS AND RETRY
================
Non-success responses throw `NotionApiException`:

    class NotionApiException : Exception
        NotionApiException(HttpStatusCode statusCode,
                           NotionAPIErrorCode? notionAPIErrorCode, string message)
        NotionAPIErrorCode? NotionAPIErrorCode { get; }
        HttpStatusCode      StatusCode         { get; }

HTTP 429 throws the derived `NotionApiRateLimitException`, which adds
`TimeSpan? RetryAfter { get; }` taken from the Retry-After header.

`NotionAPIErrorCode` is an extensible string-enum struct (unknown codes from
Notion round-trip instead of throwing). Members include `InvalidJSON`,
`InvalidRequestUrl`, `InvalidRequest`, `InvalidGrant`, `ValidationError`,
`MissingVersion`, `Unauthorized`, `RestrictedResource`, `ObjectNotFound`,
`ConflictError`, `RateLimited`, `InternalServerError`, `BadGateway`,
`ServiceUnavailable`, `DatabaseConnectionUnavailable`, `GatewayTimeout`, each
with a matching `...Value` const string.

Retry is OPT-IN via `ClientOptions.RetryPolicy`:

    interface IRetryPolicy
        bool     ShouldRetry(
            HttpResponseMessage response, HttpMethod method, int attempt)
        TimeSpan GetDelay(HttpResponseMessage response, int attempt)

    class DefaultRetryPolicy : IRetryPolicy
        DefaultRetryPolicy(int maxRetries = 3,
                           TimeSpan? initialDelay = null,   // default 1 s
                           TimeSpan? maxDelay = null)       // default 60 s
        int      MaxRetries   { get; }
        TimeSpan InitialDelay { get; }
        TimeSpan MaxDelay     { get; }
        virtual bool ShouldRetry(...)   // override to customise
        virtual TimeSpan GetDelay(...)

DefaultRetryPolicy retries 429 for ALL methods (honouring Retry-After), and 500
/ 503 only for idempotent methods (GET, DELETE), with exponential backoff plus
jitter. Retry is applied inside RestClient, so it works whether the HttpClient
is yours or the library's. Do not ALSO put a retry handler (e.g. Polly) in the
HttpClient pipeline — you would get nested retries.

SERIALIZATION
=============
All JSON goes through System.Text.Json with camelCase property naming,
`JsonIgnoreCondition.WhenWritingNull`, case-insensitive matching, and an
internal `RuntimeTypeConverterFactory` that serializes abstract- or
interface-declared values by their RUNTIME type (System.Text.Json otherwise
writes the declared type — Newtonsoft.Json, which the upstream code was
designed around, wrote the runtime type). Every request model relies on this,
so do not serialize these types with your own bare JsonSerializerOptions.

Open string-enum structs (`BlockType`, `Color`, `ObjectType`,
`PropertyValueType`, `PropertyType`, `RichTextType`, `NotionAPIErrorCode`,
`VerificationStatus`) use `ExtensibleEnumConverter<T>`: an unrecognised value
from Notion is preserved verbatim instead of throwing. Each has `...Value`
const strings for attribute use, `static readonly` fields for code, an implicit
conversion from string, and value equality.

Real C# enums in the surface — `Direction`, `Timestamp`, `QueryResultType`,
`SearchDirection`, `SearchObjectType`, `FileUploadMode`, `RelationType`,
`StatusPropertyValue.StatusColor` — serialize through
`[JsonStringEnumMemberName]`. `Direction`, `Timestamp` and `QueryResultType`
each have an `Unknown` member as their zero value; leaving them unset therefore
means "unknown", not "ascending"/"created_time", so set them explicitly.

OBJECT MODEL FOUNDATIONS
========================
IObject / ObjectType
--------------------
    interface IObject
        string     Id     { get; set; }
        ObjectType Object { get; }

`ObjectType` values: `Page`, `Database`, `Block`, `User`, `Comment`,
`FileUpload`, `DataSource`, `PageMarkdown`, `View` (plus `...Value` consts). IObject
dispatches polymorphically on the JSON `"object"` field to `Page`, `Database`,
`IBlock`, `User` or `PageMarkdownResponse`, falling back to `UnknownObject`.

    interface IObjectModificationData        // implemented by Page, Database,
        DateTime    CreatedTime    { get; set; }   // DataSource, IBlock
        DateTime    LastEditedTime { get; set; }
        PartialUser CreatedBy      { get; set; }
        PartialUser LastEditedBy   { get; set; }

Pagination
----------
    interface IPaginationParameters
        string StartCursor { get; set; }
        int?   PageSize    { get; set; }        // Notion maximum is 100

    class PaginatedList<T>
        List<T> Results    { get; set; }
        bool    HasMore    { get; set; }
        string  NextCursor { get; set; }
        string  Type       { get; set; }

Concrete paginated responses, all `PaginatedList<T>` subclasses:
    QueryDataSourceResponse   : PaginatedList<IQueryDataSourceResponseObject>
    SearchResponse            : PaginatedList<ISearchResponseObject>
    RetrieveChildrenResponse  : PaginatedList<IBlock>
    AppendChildrenResponse    : PaginatedList<IBlock>
    ListUsersResponse         : PaginatedList<User>
    RetrieveCommentsResponse  : PaginatedList<Comment>
    ListFileUploadsResponse   : PaginatedList<FileUpload>
    ListEmojisResponse        : PaginatedList<CustomEmoji>
    QueryMeetingNotesResponse : PaginatedList<MeetingNotesBlock>

The pagination loop is always the same shape:

    string cursor = null;
    do
    {
        var page = await notion.Blocks.RetrieveChildrenAsync(
            new BlockRetrieveChildrenRequest
            {
                BlockId = blockId, StartCursor = cursor, PageSize = 100,
            });

        foreach (var block in page.Results) { /* ... */ }

        cursor = page.HasMore ? page.NextCursor : null;
    }
    while (cursor != null);

`ListDataSourceTemplatesResponse` is NOT a PaginatedList; it exposes
`IEnumerable<DataSourceTemplate> Templates`, `bool HasMore`,
`string NextCursor` directly (`DataSourceTemplate` has `Id`, `Name`,
`IsDefault`).

Parents (responses) — polymorphic on "type"
-------------------------------------------
    class PageParent       : IParentOfDatabase, IParentOfBlock, IParentOfPage,
                             IParentOfComment          // string PageId
    class DatabaseParent   : IParentOfDatasource, IParentOfDatabase,
                             IParentOfBlock, IParentOfPage   // string DatabaseId
    class DatasourceParent : IParentOfDatasource, IParentOfBlock, IParentOfPage
                             // string DataSourceId, string DatabaseId
    class BlockParent      : IParentOfDatabase, IParentOfBlock, IParentOfPage,
                             IParentOfComment          // string BlockId
    class WorkspaceParent  : IParentOfDatabase, IParentOfBlock, IParentOfPage
                             // bool Workspace (always true)

Each also has `string Type` and a `[JsonExtensionData]`
`IDictionary<string, object> AdditionalData`. The parent interfaces are
`IParentOfPage`, `IParentOfBlock`, `IParentOfDatabase`, `IParentOfDatasource`,
`IParentOfComment`. `ParentTypes` holds the discriminator strings:
`Database = "database_id"`, `Page = "page_id"`, `Workspace = "workspace"`,
`Block = "block_id"`, `Datasource = "data_source_id"`.

Parents (requests) — a SEPARATE, smaller family
-----------------------------------------------
    interface IParentOfPageRequest        { string Type { get; } ... }
    interface IParentOfDataSourceRequest  { string Type { get; } ... }

    class PageParentRequest       : IParentOfPageRequest        // PageId
    class DatabaseParentRequest   : IParentOfDataSourceRequest,
                                    IParentOfPageRequest        // DatabaseId
    class DataSourceParentRequest : IParentOfPageRequest,
                                    IParentOfDataSourceRequest  // DataSourceId,
                                                                // DatabaseId
    class WorkspaceParentRequest  : IParentOfPageRequest        // Workspace

Creating a DATABASE uses yet another pair:
`PageParentOfDatabaseRequest { string PageId }` and
`WorkspaceParentOfDatabaseRequest`, both `IParentOfDatabaseRequest`.

Icons and covers
----------------
Responses: `IPageIcon` -> `EmojiPageIcon`, `CustomEmojiPageIcon`,
`IconPageIcon`, `FilePageIcon`, `ExternalPageIcon` (fallback
`ExternalPageIcon`); helper types `CustomEmoji`, `IconObject`,
`ExternalFileInfo`, `InternalFileInfo`; discriminators in `PageIconTypes`
(`Emoji`, `Icon`, `CustomEmoji`, `File`, `External`).
`IPageCover` -> `ExternalPageCover`, `FilePageCover`. (`IconObject` is nested
inside `IconPageIcon`.)

Requests: `IPageIconRequest` -> `EmojiPageIconRequest { string Emoji }`,
`CustomEmojiPageIconRequest { CustomEmojiRequest CustomEmoji }`,
`ExternalPageIconRequest { ExternalUrl External }`,
`FileUploadIconRequest { FileUploadRequest FileUpload }`.
`IPageCoverRequest` -> `ExternalPageCoverRequest { Info External }`,
`FileUploadPageCoverRequest { Info File }`; discriminators in
`PageCoverRequestTypes` (`External = "external"`,
`FileUpload = "file_upload"`).

Files
-----
    abstract class FileObject                  // Caption, Name, Type
        UploadedFile   { Info File }           // "file"      Url, ExpiryTime
        NewFileUpload  { Info FileUpload }     // "file_upload"  Id
        ExternalFile   { Info External }       // "external"  Url
        UnknownFileObject                      // fallback

    abstract class FileObjectWithName          // used by FilesPropertyValue
        UploadedFileWithName { Info File }
        UploadedFileWithId   { Info FileUpload }
        ExternalFileWithName { Info External }
        UnknownFileObjectWithName

    interface IFileObjectInput                 // used by ImageUpdateBlock etc.
        ExternalFileInput { Data External }     // Url
        UploadedFileInput { Data File }         // Url, ExpiryTime

Users
-----
    class User : IObject
        string Type, Name, AvatarUrl; Person Person; Bot Bot; string Id
    class PartialUser : IObject                // just Id + Object
    class Person { string Email }
    class Bot    { IBotOwner Owner }
    interface IBotOwner -> UserOwner { User User }
                        | WorkspaceIntegrationOwner { bool Workspace }

RICH TEXT
=========
Responses use the `RichTextBase` family; request bodies that Notion validates
strictly use the `RichTextBaseInput` family. Both share the same JSON shape.

    class RichTextBase                        // polymorphic on "type"
        string       PlainText   { get; set; }
        string       Href        { get; set; }
        Annotations  Annotations { get; set; }
        RichTextType Type        { get; set; }   // Text | Mention | Equation

    class Annotations
        bool IsBold, IsItalic, IsStrikeThrough, IsUnderline, IsCode
        Color? Color

    class RichTextText     : RichTextBase { Text Text }
        class Text { string Content; Link Link }
        class Link { string Type => "url"; string Url }
    class RichTextEquation : RichTextBase { Equation Equation }   // Expression
    class RichTextMention  : RichTextBase { Mention Mention }
        class Mention { string Type; User User; ObjectId Page;
                        ObjectId Database; Date Date }
    class ObjectId { string Id }
    class UnknownRichText  : RichTextBase        // fallback

    class RichTextBaseInput     : RichTextBase
    class RichTextTextInput     : RichTextBaseInput { Text Text }
    class RichTextEquationInput : RichTextBaseInput { Equation Equation }
    class RichTextMentionInput  : RichTextBaseInput { MentionInput Mention }
    class MentionInput { string Type; Person User; ObjectId Page;
                         ObjectId Database; Date Date }

`Color` is an extensible string-enum struct: `Default`, `Gray`, `Brown`,
`Orange`, `Yellow`, `Green`, `Blue`, `Purple`, `Pink`, `Red` and the matching
`...Background` values.

Build runs with `NotionText` (see "AUTHORING HELPERS") rather than by hand —
it enforces Notion's 2,000-character-per-run limit for you.

PAGE PROPERTIES — READING AND WRITING
=====================================
There are THREE distinct property families. Confusing them is the single most
common mistake with this library:

  * PropertyValue      — a property's VALUE on a page. `Page.Properties` is
                         `IDictionary<string, PropertyValue>` keyed by property
                         NAME (or id). This is what you read AND what you write.
  * IPropertyItemObject — the response of
                         `Pages.RetrievePagePropertyItemAsync`, used when a
                         single property is too large to inline (relations,
                         rollups, long rich text). Paginated.
  * Property / DataSourcePropertyConfig — the SCHEMA (type, options, formula
                         expression) of a column, not a value. See "SCHEMA".

PropertyValue — the value family
--------------------------------
    class PropertyValue                        // polymorphic on "type"
        string            Id   { get; set; }
        PropertyValueType Type { get; }        // virtual, overridden per type

Concrete types and their payload property (all `: PropertyValue`):

    TitlePropertyValue          List<RichTextBase>       Title
    RichTextPropertyValue       List<RichTextBase>       RichText
    SelectPropertyValue         SelectOption             Select
    MultiSelectPropertyValue    List<SelectOption>       MultiSelect
    StatusPropertyValue         StatusPropertyValue.Data Status
    DatePropertyValue           Date                     Date
    NumberPropertyValue         double?                  Number
    CheckboxPropertyValue       bool                     Checkbox
    PeoplePropertyValue         List<User>               People
    RelationPropertyValue       List<ObjectId>           Relation
    FilesPropertyValue          List<FileObjectWithName> Files
    UrlPropertyValue            string                   Url
    EmailPropertyValue          string                   Email
    PhoneNumberPropertyValue    string                   PhoneNumber
    FormulaPropertyValue        FormulaValue             Formula     (read-only)
    RollupPropertyValue         RollupValue              Rollup      (read-only)
    CreatedTimePropertyValue    string                   CreatedTime (read-only)
    CreatedByPropertyValue      User                     CreatedBy   (read-only)
    LastEditedTimePropertyValue string                 LastEditedTime(read-only)
    LastEditedByPropertyValue   User                     LastEditedBy(read-only)
    UniqueIdPropertyValue       UniqueIdValue            UniqueId    (read-only)
    VerificationPropertyValue   VerificationPropertyValue.Info Verification
    ButtonPropertyValue         ButtonPropertyValue.ButtonValue Button
    PlacePropertyValue          PlacePropertyValue.PlaceInfo    Place
    UnknownPropertyValue        (fallback for unrecognised "type")

Supporting shapes:

    class Date                                  // shared by date filters/values
        DateTimeOffset? Start; DateTimeOffset? End;
        string TimeZone; bool IncludeTime = true
    class SelectOption      { string Name; string Id; Color? Color }
    class StatusPropertyValue.Data { string Id; string Name;
                                     StatusColor? Color }
    enum StatusPropertyValue.StatusColor { Default, Gray, Brown, Orange,
                                     Yellow, Green, Blue, Purple, ... }
    class FormulaValue      { string Type; string String; double? Number;
                              bool? Boolean; Date Date }
    class RollupValue       { string Type; double? Number; Date Date;
                              List<PropertyValue> Array }
    class UniqueIdValue     { string Prefix; double? Number }
    class VerificationPropertyValue.Info { VerificationStatus State;
                                           User VerifiedBy; Date Date }
        (`State` was a plain `string` before API 2026-03-11. VerificationStatus
         is an extensible string-enum struct: Verified | Unverified | Expired |
         None. Notion READS back Verified / Expired / None and ACCEPTS
         Verified / Unverified on a write.)
    class PlacePropertyValue.PlaceInfo { double Lat; double Lon; string Name;
                              string Address; string AwsPlaceId;
                              string GooglePlaceId }

`PropertyValueType` (extensible string-enum struct) values: `Title`, `RichText`,
`Number`, `Select`, `MultiSelect`, `Status`, `Date`, `People`, `Files`,
`Checkbox`, `Url`, `Email`, `PhoneNumber`, `Formula`, `Relation`, `Rollup`,
`CreatedTime`, `CreatedBy`, `LastEditedTime`, `LastEditedBy`, `UniqueId`,
`Verification`, `Button`, `Place`.

WRITING properties — build a dictionary keyed by the property NAME exactly as
it appears in Notion (a page whose parent is a page accepts only "title"):

    var properties = new Dictionary<string, PropertyValue>
    {
        ["Name"]   = new TitlePropertyValue
                     { Title = NotionText.PlainBase("Quarterly report") },
        ["Notes"]  = new RichTextPropertyValue
                     { RichText = NotionText.PlainBase("Draft") },
        ["Status"] = new StatusPropertyValue
                     { Status = new StatusPropertyValue.Data
                       { Name = "In progress" } },
        ["Tags"]   = new MultiSelectPropertyValue
                     { MultiSelect = new List<SelectOption>
                       { new SelectOption { Name = "finance" } } },
        ["Stage"]  = new SelectPropertyValue
                     { Select = new SelectOption { Name = "Review" } },
        ["Due"]    = new DatePropertyValue
                     { Date = new Date
                       { Start = DateTimeOffset.UtcNow.AddDays(7),
                         IncludeTime = false } },
        ["Points"] = new NumberPropertyValue  { Number = 12.5 },
        ["Done"]   = new CheckboxPropertyValue { Checkbox = false },
        ["Owner"]  = new PeoplePropertyValue
                     { People = new List<User> { new User { Id = userId } } },
        ["Parent task"] = new RelationPropertyValue
                     { Relation = new List<ObjectId>
                       { new ObjectId { Id = otherPageId } } },
        ["Docs"]   = new UrlPropertyValue   { Url = "https://example.com" },
        ["Email"]  = new EmailPropertyValue { Email = "a@example.com" },
    };

    await notion.Pages.UpdatePropertiesAsync(pageId, properties);
    // or: notion.Pages.UpdateAsync(pageId,
    //         new PagesUpdateParameters { Properties = properties });

Properties you do NOT include are left unchanged. Formula, rollup, created/last
edited and unique-id values are computed by Notion and cannot be written.

READING properties — pattern-match on the concrete type:

    var page = await notion.Pages.RetrieveAsync(pageId);

    foreach (var entry in page.Properties)
    {
        switch (entry.Value)
        {
            case TitlePropertyValue title:
                Console.WriteLine($"{entry.Key}: " +
                    string.Concat(title.Title.Select(r => r.PlainText)));
                break;
            case RichTextPropertyValue text:
                Console.WriteLine($"{entry.Key}: " +
                    string.Concat(text.RichText.Select(r => r.PlainText)));
                break;
            case SelectPropertyValue select:
                Console.WriteLine($"{entry.Key}: {select.Select?.Name}");
                break;
            case MultiSelectPropertyValue multi:
                Console.WriteLine($"{entry.Key}: " +
                    string.Join(", ", multi.MultiSelect.Select(o => o.Name)));
                break;
            case DatePropertyValue date:
                Console.WriteLine($"{entry.Key}: {date.Date?.Start:d}");
                break;
            case NumberPropertyValue number:
                Console.WriteLine($"{entry.Key}: {number.Number}");
                break;
            case CheckboxPropertyValue check:
                Console.WriteLine($"{entry.Key}: {check.Checkbox}");
                break;
            case RelationPropertyValue relation:
                Console.WriteLine($"{entry.Key}: " +
                    string.Join(", ", relation.Relation.Select(r => r.Id)));
                break;
            case FormulaPropertyValue formula:
                Console.WriteLine($"{entry.Key}: {formula.Formula?.String}");
                break;
            case UnknownPropertyValue:
                // a property type newer than this library — value not modelled
                break;
        }
    }

Property ITEMS — the paginated read path
----------------------------------------
Notion truncates large property values inside a Page response. Fetch the full
value with `Pages.RetrievePagePropertyItemAsync`:

    class RetrievePropertyItemParameters : IRetrievePropertyItemPathParameters,
                                           IRetrievePropertyQueryParameters
        string PageId, PropertyId, StartCursor; int? PageSize

    interface IPropertyItemObject
        string Object, Type, Id, NextURL { get; }

    abstract class SimplePropertyItem : IPropertyItemObject
                                              // Object == "property_item"
    class ListPropertyItem : IPropertyItemObject              // Object=="list"
        IEnumerable<SimplePropertyItem> Results
        bool HasMore; string NextCursor
        SimplePropertyItem PropertyItem      // describes the element type

Concrete `SimplePropertyItem` subclasses (payload in parentheses):
`TitlePropertyItem` (RichTextBase Title), `RichTextPropertyItem` (RichTextBase
RichText), `NumberPropertyItem` (double? Number), `SelectPropertyItem`,
`MultiSelectPropertyItem`, `StatusPropertyItem`, `DatePropertyItem`,
`EmailPropertyItem`, `PhoneNumberPropertyItem`, `UrlPropertyItem`,
`CheckboxPropertyItem`, `FilesPropertyItem`, `PeoplePropertyItem`,
`RelationPropertyItem`, `RollupPropertyItem` (RollupPropertyItem.Data),
`FormulaPropertyItem`, `CreatedByPropertyItem`, `CreatedTimePropertyItem`,
`LastEditedByPropertyItem`, `LastEditedTimePropertyItem`,
`PlacePropertyItem` (PlacePropertyValue.PlaceInfo Place),
`UnknownPropertyItem` (fallback).

    var item = await notion.Pages.RetrievePagePropertyItemAsync(
        new RetrievePropertyItemParameters
        {
            PageId = pageId, PropertyId = propertyId, PageSize = 100,
        });

    if (item is ListPropertyItem list)
    {
        foreach (var element in list.Results) { /* per-type switch */ }
    }
    else if (item is NumberPropertyItem number)
    {
        Console.WriteLine(number.Number);
    }

QUERYING A DATA SOURCE
======================
Under the 2026-03-11 API, rows live in a DATA SOURCE, and a database points at
one or more of them. The full round trip:

    var database = await notion.Databases.RetrieveAsync(databaseId);
    var dataSourceId = database.DataSources.First().DataSourceId;

The request object
------------------
    class QueryDataSourceRequest : IQueryDataSourcePathParameters,
                                   IQueryDataSourceQueryParameters,
                                   IQueryDataSourceBodyParameters
        string              DataSourceId     { get; set; }   // path
        IEnumerable<string> FilterProperties { get; set; }   // query string
        IEnumerable<Sort>   Sorts            { get; set; }
        Filter              Filter           { get; set; }
        string              StartCursor      { get; set; }
        int?                PageSize         { get; set; }   // max 100
        bool?               Archived         { get; set; }
        bool?               InTrash          { get; set; }
        QueryResultType?    ResultType       { get; set; }   // Page | DataSource

    class QueryDataSourceResponse : PaginatedList<IQueryDataSourceResponseObject>
        Dictionary<string, object> PageOrDataSource
        IDictionary<string, object> AdditionalData   // JsonExtensionData

`IQueryDataSourceResponseObject : IObject` resolves to `Page` or `DataSource`
(fallback `UnknownObject`), so `response.Results.OfType<Page>()` is the normal
way to consume it.

Sorts
-----
    class Sort
        string    Property  { get; set; }   // sort by a property name
        Timestamp Timestamp { get; set; }   // OR by CreatedTime/LastEditedTime
        Direction Direction { get; set; }

    enum Direction { Unknown, Ascending, Descending }
    enum Timestamp { Unknown, CreatedTime, LastEditedTime }

Set exactly one of Property / Timestamp per Sort, and ALWAYS set Direction —
its default value is `Unknown`.

Filters
-------
`Filter` is the abstract base. `SinglePropertyFilter : Filter` adds
`string Property`; `CompoundFilter : Filter` nests groups:

    class CompoundFilter : Filter
        CompoundFilter(List<Filter> or = null, List<Filter> and = null)
        List<Filter> Or { get; set; }
        List<Filter> And { get; set; }

Every property filter takes the property name first, then named optional
condition arguments, and exposes the condition object as a settable property:

    TitleFilter(string propertyName, string equal = null,
        string doesNotEqual = null, string contains = null,
        string doesNotContain = null, string startsWith = null,
        string endsWith = null, bool? isEmpty = null, bool? isNotEmpty = null)
    RichTextFilter(string propertyName, ... same eight text conditions ...)
    URLFilter(string propertyName, ... same eight ...)
    EmailFilter(string propertyName, ... same eight ...)
    PhoneNumberFilter(string propertyName, ... same eight ...)

    SelectFilter(string propertyName, string equal = null,
        string doesNotEqual = null, bool? isEmpty = null, bool? isNotEmpty = null)
    StatusFilter(string propertyName, ... same four as SelectFilter ...)
    MultiSelectFilter(string propertyName, string contains = null,
        string doesNotContain = null, bool? isEmpty = null, bool? isNotEmpty = null)
    RelationFilter(string propertyName, ... same four as MultiSelectFilter ...)
    PeopleFilter(string propertyName, ... same four as MultiSelectFilter ...)

    NumberFilter(string propertyName, double? equal = null,
        double? doesNotEqual = null, double? greaterThan = null,
        double? lessThan = null, double? greaterThanOrEqualTo = null,
        double? lessThanOrEqualTo = null, bool? isEmpty = null,
        bool? isNotEmpty = null)
    CheckboxFilter(string propertyName, bool? equal = null,
        bool? doesNotEqual = null)
    FilesFilter(string propertyName, bool? isEmpty = null,
        bool? isNotEmpty = null)

    DateFilter(string propertyName, RelativeDateValue? equal = null,
        RelativeDateValue? before = null, RelativeDateValue? after = null,
        RelativeDateValue? onOrBefore = null, RelativeDateValue? onOrAfter = null,
        Dictionary<string, object> pastWeek = null,
        Dictionary<string, object> pastMonth = null,
        Dictionary<string, object> pastYear = null,
        Dictionary<string, object> nextWeek = null,
        Dictionary<string, object> nextMonth = null,
        Dictionary<string, object> nextYear = null,
        bool? isEmpty = null, bool? isNotEmpty = null)

The pastWeek / pastMonth / pastYear / nextWeek / nextMonth / nextYear arguments
are marker objects: pass `new Dictionary<string, object>()` to select them
(Notion expects an empty JSON object there).

The five comparison arguments take `RelativeDateValue`, an extensible
string-enum struct with IMPLICIT conversions from `DateTime`, `DateTimeOffset`
and `string`, so existing `DateTime` call sites keep compiling:

    new DateFilter("Due", onOrBefore: DateTime.UtcNow.AddDays(30))
    new DateFilter("Due", onOrAfter: RelativeDateValue.Today)
    new DateFilter("Due", before: RelativeDateValue.OneWeekFromNow)

    RelativeDateValue keywords: Today, Tomorrow, Yesterday, OneWeekAgo,
    OneWeekFromNow, OneMonthAgo, OneMonthFromNow (each with a `...Value` const).

    A `DateTime` converts using its Kind: Utc -> "...Z", Local -> "...+hh:mm",
    Unspecified -> no suffix. A `DateTimeOffset` always keeps its offset. Note
    that System.Text.Json's default encoder escapes "+" as \u002B in the
    serialized filter — that is still valid JSON and Notion decodes it back.

Filters whose condition is supplied as an already-built Condition object:

    CreatedTimeFilter(string propertyName, DateFilter.Condition createdTime)
    LastEditedTimeFilter(string propertyName, DateFilter.Condition lastEditedTime)
    CreatedByFilter(string propertyName, PeopleFilter.Condition createdBy)
    LastEditedByFilter(string propertyName, PeopleFilter.Condition lastEditedBy)
    UniqueIdFilter(string propertyName, NumberFilter.Condition uniqueId)
    VerificationPropertyStatusFilter(string propertyName,
        VerificationPropertyStatusFilter.Condition verificationStatus)

    FormulaFilter(string propertyName, TextFilter.Condition @string = null,
        CheckboxFilter.Condition checkbox = null,
        NumberFilter.Condition number = null,
        DateFilter.Condition date = null)
    RollupFilter(string propertyName, IRollupSubPropertyFilter any = null,
        IRollupSubPropertyFilter none = null,
        IRollupSubPropertyFilter every = null,
        DateFilter.Condition date = null, NumberFilter.Condition number = null)

`TextFilter` is a static holder for the shared `TextFilter.Condition` (equals /
does_not_equal / contains / does_not_contain / starts_with / ends_with /
is_empty / is_not_empty) used by TitleFilter, RichTextFilter, URLFilter,
EmailFilter and PhoneNumberFilter — there is no `new TextFilter(...)`.
The filters that may appear inside a rollup (`IRollupSubPropertyFilter`) are
RichTextFilter, SelectFilter, MultiSelectFilter, StatusFilter, NumberFilter,
CheckboxFilter, DateFilter, RelationFilter, PeopleFilter, FilesFilter.

Timestamp filters are NOT property filters and derive straight from `Filter`:

    TimestampCreatedTimeFilter(DateTime? equal = null, ... same arguments as
        DateFilter minus propertyName ...)
    TimestampLastEditedTimeFilter(... same ...)

`VerificationStatus` is an extensible string-enum struct with `Verified`,
`Expired`, `None`.

SCHEMA — CREATING AND UPDATING DATABASES AND DATA SOURCES
=========================================================
Response side: a `DataSource` carries
`IDictionary<string, DataSourcePropertyConfig> Properties`.

    class DataSourcePropertyConfig            // polymorphic on "type"
        string Id, Type, Name, Description
        IDictionary<string, object> AdditionalData

`StatusDataSourcePropertyConfig.Status` is a TYPED `StatusConfig` (it was a
`Dictionary<string, object>` before API 2026-03-11):

    class StatusConfig  { IEnumerable<StatusOption> Options;
                          IEnumerable<StatusGroup> Groups;
                          IDictionary<string, object> AdditionalData }
    class StatusOption  { string Id, Name; Color? Color }
    class StatusGroup   { string Id, Name; Color? Color;
                          IEnumerable<string> OptionIds }

`StatusProperty.Status` (the database-level property family) uses the same
`StatusConfig`.

Concrete configs: `TitleDataSourcePropertyConfig`,
`RichTextDataSourcePropertyConfig`, `NumberDataSourcePropertyConfig`
(NumberResponse), `SelectDataSourcePropertyConfig` (SelectOptionResponse),
`MultiSelectDataSourcePropertyConfig`, `StatusDataSourcePropertyConfig`,
`DateDataSourcePropertyConfig`, `PeopleDataSourcePropertyConfig`,
`FilesDataSourcePropertyConfig`, `CheckboxDataSourcePropertyConfig`,
`UrlDataSourcePropertyConfig`, `EmailDataSourcePropertyConfig`,
`PhoneNumberDataSourcePropertyConfig`, `FormulaDataSourcePropertyConfig`
(FormulaResponse), `RelationDataSourcePropertyConfig` (RelationInfo ->
`SinglePropertyRelationInfo` | `DualPropertyRelationInfo`),
`RollupDataSourcePropertyConfig` (RollupResponse),
`CreatedTimeDataSourcePropertyConfig`, `CreatedByDataSourcePropertyConfig`,
`LastEditedTimeDataSourcePropertyConfig`,
`LastEditedByDataSourcePropertyConfig`, `UniqueIdDataSourcePropertyConfig`,
`ButtonDataSourcePropertyConfig`, `PlaceDataSourcePropertyConfig`,
`UnknownDataSourcePropertyConfig` (fallback). The discriminator strings are in
`DataSourcePropertyTypes` (`Title`, `RichText`, `Number`, `Select`,
`MultiSelect`, `Date`, `People`, `Files`, `Checkbox`, `Url`, `Email`,
`PhoneNumber`, `Formula`, `Relation`, `Rollup`, `CreatedTime`, `CreatedBy`,
`LastEditedBy`, `LastEditedTime`, `Status`, `UniqueId`, `Button`, `Place`).

Request side: a PARALLEL `...Request` family.

    abstract class DataSourcePropertyConfigRequest
        virtual string Type { get; set; }
        string Description { get; set; }
        IDictionary<string, object> AdditionalData

    (ABSTRACT: always construct one of the concrete `...Request` subclasses
     below. System.Text.Json serializes the DECLARED type, so a concrete base
     here would drop the per-type payload when the value sits in an
     `IDictionary<string, DataSourcePropertyConfigRequest>`.)

`StatusDataSourcePropertyConfigRequest.Status` is a typed `StatusConfigRequest`
(it was an `IDictionary<string, object>` before API 2026-03-11):

    class StatusConfigRequest { IEnumerable<StatusOptionRequest> Options;
                                IEnumerable<StatusGroupRequest> Groups }
    class StatusOptionRequest { string Id, Name; Color? Color }
    class StatusGroupRequest  { string Id, Name; Color? Color;
                                IEnumerable<string> OptionIds }

Concrete requests: `TitleDataSourcePropertyConfigRequest`,
`RichTextDataSourcePropertyConfigRequest`,
`NumberDataSourcePropertyConfigRequest` (NumberFormat Number { string Format }),
`SelectDataSourcePropertyConfigRequest` (SelectOptions Select { Options }),
`MultiSelectDataSourcePropertyConfigRequest` (MultiSelectOptions MultiSelect),
`StatusDataSourcePropertyConfigRequest`,
`DateDataSourcePropertyConfigRequest`, `PeopleDataSourcePropertyConfigRequest`,
`FilesDataSourcePropertyConfigRequest`,
`CheckboxDataSourcePropertyConfigRequest`,
`UrlDataSourcePropertyConfigRequest`, `EmailDataSourcePropertyConfigRequest`,
`PhoneNumberDataSourcePropertyConfigRequest`,
`FormulaDataSourcePropertyConfigRequest` (FormulaPropertyConfiguration),
`RelationDataSourcePropertyConfigRequest` (IRelationInfoRequest ->
`SinglePropertyRelationInfoRequest` | `DualPropertyRelationInfoRequest`, both
carrying `DataSourceId`), `RollupDataSourcePropertyConfigRequest`
(RollupOptions), `CreatedTimeDataSourcePropertyConfigRequest`,
`CreatedByDataSourcePropertyConfigRequest`,
`LastEditedTimeDataSourcePropertyConfigRequest`,
`LastEditedByDataSourcePropertyConfigRequest`,
`LastVisitedTimeDataSourcePropertyConfigRequest`,
`LocationDataSourcePropertyConfigRequest`,
`PlaceDataSourcePropertyConfigRequest`,
`UniqueIdDataSourcePropertyConfigRequest` (UniqueIdConfiguration),
`VerificationDataSourcePropertyConfigRequest`,
`ButtonDataSourcePropertyConfigRequest`.
Options are `SelectOptionRequest { string Name; string Color;
string Description }`.

Creating a database with its initial schema:

    class DatabasesCreateRequest : IDatabasesCreateBodyParameters
        IParentOfDatabaseRequest    Parent
        List<RichTextBaseInput>     Title
        List<RichTextBaseInput>     Description
        bool?                       IsInline
        InitialDataSourceRequest    InitialDataSource
        IPageIconRequest            Icon
        IPageCoverRequest           Cover

    class InitialDataSourceRequest          // a TOP-LEVEL type, not nested
        Dictionary<string, DataSourcePropertyConfigRequest> Properties

    var database = await notion.Databases.CreateAsync(new DatabasesCreateRequest
    {
        Parent = new PageParentOfDatabaseRequest { PageId = parentPageId },
        Title = new List<RichTextBaseInput>
        {
            new RichTextTextInput { Text = new Text { Content = "Tasks" } },
        },
        InitialDataSource = new InitialDataSourceRequest
        {
            Properties = new Dictionary<string, DataSourcePropertyConfigRequest>
            {
                ["Name"]   = new TitleDataSourcePropertyConfigRequest(),
                ["Notes"]  = new RichTextDataSourcePropertyConfigRequest(),
                ["Points"] = new NumberDataSourcePropertyConfigRequest
                {
                    Number = new NumberDataSourcePropertyConfigRequest
                        .NumberFormat { Format = "number" },
                },
                ["Stage"]  = new SelectDataSourcePropertyConfigRequest
                {
                    Select = new SelectDataSourcePropertyConfigRequest
                        .SelectOptions
                    {
                        Options = new[]
                        {
                            new SelectOptionRequest
                                { Name = "Todo",  Color = "gray" },
                            new SelectOptionRequest
                                { Name = "Doing", Color = "blue" },
                        },
                    },
                },
                ["Done"]   = new CheckboxDataSourcePropertyConfigRequest(),
                ["Due"]    = new DateDataSourcePropertyConfigRequest(),
            },
        },
    });

    var dataSourceId = database.DataSources.First().DataSourceId;

Updating a database (title, icon, cover, in_trash, is_inline, description,
is_locked, parent) uses `DatabasesUpdateRequest { string DatabaseId; ... }`.

Data-source-level operations:

    class CreateDataSourceRequest
        IParentOfDataSourceRequest Parent            // DatabaseParentRequest
        IDictionary<string, DataSourcePropertyConfigRequest> Properties
        IEnumerable<RichTextBaseInput> Title
        IPageIconRequest Icon
    class RetrieveDataSourceRequest { string DataSourceId }
    class UpdateDataSourceRequest
        string DataSourceId
        IEnumerable<RichTextBaseInput> Title
        IPageIconRequest Icon
        IDictionary<string, IUpdatePropertyConfigurationRequest> Properties
        bool InTrash; bool Archived; IParentOfDataSourceRequest Parent
    class ListDataSourceTemplatesRequest
        string DataSourceId; string Name; string StartCursor; int? PageSize

Renaming or retyping a column goes through
`UpdatePropertyConfigurationRequest<T> : IUpdatePropertyConfigurationRequest`
(`string Name`, `T PropertyRequest` where
`T : DataSourcePropertyConfigRequest`); `PropertyRequest` is flattened into the
same JSON object as `name` when serialized.

    await notion.DataSources.UpdateAsync(new UpdateDataSourceRequest
    {
        DataSourceId = dataSourceId,
        Properties = new Dictionary<string, IUpdatePropertyConfigurationRequest>
        {
            ["Points"] = new UpdatePropertyConfigurationRequest
                <NumberDataSourcePropertyConfigRequest> { Name = "Story points" },
        },
    });

`Database` (response) exposes `Title`, `Description`, `Parent`, `IsInline`,
`InTrash`, `IsLocked`, `Icon`, `Cover`, `Url`, `PublicUrl` and
`IEnumerable<DataSourceReferenceResponse> DataSources`, where
`DataSourceReferenceResponse` is `{ string DataSourceId; string Name }`.
`DataSource` (response) exposes `Title`, `Description`, `Parent`,
`DatabaseParent`, `IsInline`, `Archived` ([Obsolete] — use `InTrash`),
`InTrash`, `Properties`, `Icon`, `Cover`, `Url`, `PublicUrl`, plus the
IObjectModificationData members.
The database-level `Property` family mirrors the same information under the
older `PropertyType` discriminator and appears where the API still returns
database properties: `TitleProperty`, `RichTextProperty`, `NumberProperty`,
`SelectProperty` and `MultiSelectProperty` (both carrying
`OptionWrapper<SelectOption>`), `StatusProperty`, `DateProperty`,
`PeopleProperty`, `FilesProperty`, `CheckboxProperty`, `UrlProperty`,
`EmailProperty`, `PhoneNumberProperty`, `FormulaProperty`, `RelationProperty`
(`RelationData` -> `SinglePropertyRelation` | `DualPropertyRelation`, with
`RelationType`), `RollupProperty`, `CreatedTimeProperty`, `CreatedByProperty`,
`LastEditedTimeProperty`, `LastEditedByProperty`, `UniqueIdProperty`,
`VerificationProperty`, `ButtonProperty`, `PlaceProperty`, and the fallback
`UnknownProperty`.

THE BLOCK MODEL LAYER
=====================
Blocks come in THREE parallel families. Pick by direction:

  * `IBlock` / `Block`             — what you READ back (responses).
  * `IBlockObjectRequest` / `BlockObjectRequest` — what you SEND when creating
                                     or appending (`...BlockRequest` types).
  * `IUpdateBlock` / `UpdateBlock` — what you SEND to `Blocks.UpdateAsync`
                                     (`...UpdateBlock` types). These carry ONLY
                                     the fields Notion lets you change.

    interface IBlock : IObject, IObjectModificationData
        BlockType     Type        { get; set; }
        bool          HasChildren { get; set; }
        bool          InTrash     { get; set; }
        IParentOfBlock Parent     { get; set; }

    abstract class Block : IBlock
        ObjectType Object => ObjectType.Block; string Id;
        virtual BlockType Type; DateTime CreatedTime, LastEditedTime;
        virtual bool HasChildren; bool InTrash;
        PartialUser CreatedBy, LastEditedBy; IParentOfBlock Parent

    interface IBlockObjectRequest : IObject, IObjectModificationData
        BlockType Type { get; }  bool HasChildren { get; set; }
        IParentOfBlock Parent { get; set; }

    abstract class BlockObjectRequest : IBlockObjectRequest    // same shape,
                                                               // no InTrash

    interface IUpdateBlock          { bool InTrash { get; set; } }
    abstract class UpdateBlock : IUpdateBlock
    (most ...UpdateBlock types derive from UpdateBlock; BookmarkUpdateBlock,
     BreadcrumbUpdateBlock, DividerUpdateBlock and TableOfContentsUpdateBlock
     implement IUpdateBlock directly. Blocks.UpdateAsync takes IUpdateBlock,
     so this makes no difference at the call site.)

Nesting marker interfaces constrain what may go where:
`INonColumnBlock` / `INonColumnBlockRequest` (anything except a column),
`IColumnChildrenBlock` / `IColumnChildrenBlockRequest` (legal inside a column),
`ITemplateChildrenBlock` / `ITemplateChildrenBlockRequest`,
`ISyncedBlockChildren` / `ISyncedBlockChildrenRequest`.

Feature-by-feature table. Read type — Request type — Update type — payload.

    paragraph
        ParagraphBlock / ParagraphBlockRequest / ParagraphUpdateBlock
        .Paragraph -> Info { RichText, Color?, Children, Icon }
        (`Icon` is `IPageIcon` on the response and `IPageIconRequest` on the
         request. It is ONLY legal on a paragraph that is a direct child of a
         tab block — setting it anywhere else is a validation error.)
    heading_1 / heading_2 / heading_3 / heading_4
        HeadingOneBlock, HeadingTwoBlock, HeadingThreeBlock, HeadingFourBlock
        HeadingOneBlockRequest, HeadingTwoBlockRequest,
        HeadingThreeBlockRequest, HeadingFourBlockRequest
        HeadingOneUpdateBlock, HeadingTwoUpdateBlock, HeadingThreeUpdateBlock,
        HeadingFourUpdateBlock
        .Heading_1 / .Heading_2 / .Heading_3 / .Heading_4 ->
            Info { RichText, Color?, IsToggleable }
        (the payload property really is named Heading_1 / Heading_2 /
         Heading_3, with an underscore)
    bulleted_list_item
        BulletedListItemBlock / ...Request / BulletedListItemUpdateBlock
        .BulletedListItem -> Info { RichText, Color?, Children }
    numbered_list_item
        NumberedListItemBlock / ...Request / NumberedListItemUpdateBlock
        .NumberedListItem -> Info { RichText, Color?, Children,
                                    int? ListStartIndex,
                                    NumberedListFormat? ListFormat }
        (ListStartIndex and ListFormat are READ-ONLY response fields — Notion
         rejects them on create/update. `NumberedListFormat` is an extensible
         string-enum struct: Numbers | Letters | Roman.)
    to_do
        ToDoBlock / ToDoBlockRequest / ToDoUpdateBlock
        .ToDo -> Info { RichText, IsChecked, Color?, Children }
    toggle
        ToggleBlock / ToggleBlockRequest / ToggleUpdateBlock
        .Toggle -> Info { RichText, Color?, Children }
    code
        CodeBlock / CodeBlockRequest / CodeUpdateBlock
        .Code -> Info { RichText, string Language, Caption }
    quote
        QuoteBlock / QuoteBlockRequest / QuoteUpdateBlock
        .Quote -> Info { RichText, Color?, Children }
    callout
        CalloutBlock / CalloutBlockRequest / CalloutUpdateBlock
        .Callout -> Info { RichText, Icon (IPageIcon on the response,
                           IPageIconRequest on the request), Color?, Children }
    image / video / audio / file / pdf
        ImageBlock, VideoBlock, AudioBlock, FileBlock, PDFBlock
        ImageBlockRequest, VideoBlockRequest, AudioBlockRequest,
        FileBlockRequest, PDFBlockRequest       (payload: FileObject)
        ImageUpdateBlock, VideoUpdateBlock, AudioUpdateBlock, FileUpdateBlock,
        PDFUpdateBlock                          (payload: IFileObjectInput)
    table / table_row
        TableBlock / TableBlockRequest / TableUpdateBlock
        .Table -> Info { int TableWidth, bool HasColumnHeader,
                         bool HasRowHeader, Children (table rows) }
        TableRowBlock / TableRowBlockRequest / TableRowUpdateBlock
        .TableRow -> Info { Cells } — a list of lists of rich text
    column_list / column
        ColumnListBlock / ColumnListBlockRequest      .ColumnList -> Info
                                                      { Children (columns) }
        ColumnBlock / ColumnBlockRequest              .Column -> Info
                                                      { Children,
                                                        double? WidthRatio }
        (WidthRatio is this column's share of the available width, e.g. 0.25
         for 25%. Settable on the request and returned on the response.)
        (a column_list may contain ONLY columns; a column may contain any
         IColumnChildrenBlock. There is no ColumnUpdateBlock.)
    divider
        DividerBlock / DividerBlockRequest / DividerUpdateBlock
        .Divider -> Data { }   (an empty marker object — it must still be set)
    breadcrumb
        BreadcrumbBlock / BreadcrumbBlockRequest / BreadcrumbUpdateBlock
        .Breadcrumb -> Data { }
    table_of_contents
        TableOfContentsBlock / TableOfContentsBlockRequest /
        TableOfContentsUpdateBlock
        .TableOfContents -> Data { Color? }
    bookmark
        BookmarkBlock / BookmarkBlockRequest / BookmarkUpdateBlock
        .Bookmark -> Info { string Url, Caption }
    embed
        EmbedBlock / EmbedBlockRequest / EmbedUpdateBlock
        .Embed -> Info { string Url, Caption }
    link_preview
        LinkPreviewBlock / LinkPreviewBlockRequest    .LinkPreview -> Data
                                                      { string Url }
    equation
        EquationBlock / EquationBlockRequest / EquationUpdateBlock
        .Equation -> Info { string Expression }
    child_page / child_database
        ChildPageBlock / ChildPageBlockRequest          .ChildPage -> Info
                                                        { string Title }
        ChildDatabaseBlock / ChildDatabaseBlockRequest  .ChildDatabase -> Info
                                                        { string Title }
    link_to_page
        LinkToPageBlock / LinkToPageBlockRequest / LinkToPageUpdateBlock
        .LinkToPage -> ILinkToPage: LinkPageToPage | LinkDatabaseToPage |
                       LinkCommentToPage
    synced_block
        SyncedBlockBlock / SyncedBlockBlockRequest / SyncedBlockUpdateBlock
        .SyncedBlock -> Data { SyncedFromBlockId SyncedFrom, Children }
        SyncedFrom == null means THIS is the original; set
        SyncedFrom = new SyncedBlockBlockRequest.SyncedFromBlockId
                     { Type = "block_id", BlockId = originalId }
        to create a duplicate view of another block.
    template
        TemplateBlock / TemplateBlockRequest / TemplateUpdateBlock
        .Template -> Data { RichText, Children }
    tab
        TabBlock / TabBlockRequest        (no update type)
        .Tab -> Data { }  on the response; on the request
        .Tab -> Data { IEnumerable<ParagraphBlockRequest> Children }
        Only PARAGRAPH blocks are legal tab children; each one is a tab, and
        each may carry a `ParagraphBlockRequest.Info.Icon`.
    meeting_notes
        MeetingNotesBlock (read only)
        .MeetingNotes -> MeetingNotesBlockData
            { Title, string Status, MeetingNotesChildrenData Children,
              MeetingNotesCalendarEventData CalendarEvent,
              MeetingNotesRecordingData Recording }
            MeetingNotesChildrenData { SummaryBlockId, NotesBlockId,
                                       TranscriptBlockId }
            MeetingNotesCalendarEventData { StartTime, EndTime, Attendees }
            MeetingNotesRecordingData     { StartTime, EndTime }
        Query them with `notion.Blocks.QueryMeetingNotesAsync` — that endpoint
        requires a Notion plan with AI meeting notes enabled.
    transcription   [OBSOLETE — renamed meeting_notes in API 2026-03-11]
        TranscriptionBlock (read only; TranscriptionBlockResponse,
        TranscriptionChildrenResponse, TranscriptionRecordingResponse,
        TranscriptionCalendarEventResponse). Still registered, so a payload
        from a workspace pinned to an older API version deserializes.
    unsupported
        UnsupportedBlock — the FALLBACK for any block type this library does
        not model; `.Unsupported` is `UnsupportedBlockResponse
        { string BlockType }`. Deserialization never throws on a new Notion
        block type; you get one of these instead.

`BlockType` (extensible string-enum struct) covers `Paragraph`, `Heading1`,
`Heading2`, `Heading3`, `BulletedListItem`, `NumberedListItem`, `ToDo`,
`Toggle`, `ChildPage`, `Code`, `ChildDatabase`, `Embed`, `Image`, `Video`,
`File`, `PDF`, `Bookmark`, `Equation`, `Breadcrumb`, `Divider`, `Audio`,
`TableOfContents`, `Callout`, `Quote`, `Column`, `ColumnList`, `Template`,
`LinkToPage`, `SyncedBlock`, `Table`, `TableRow`, `LinkPreview`, `Unsupported`,
`Heading4`, `Tab`, `MeetingNotes` and the obsolete `Transcription`, each with a
`...Value` const string.

Appending, updating and deleting blocks
---------------------------------------
    class BlockAppendChildrenRequest
        string BlockId                            // the PARENT page or block
        IEnumerable<IBlockObjectRequest> Children // max 100 per request
        ContentPosition Position                  // where to put them

    BREAKING in API 2026-03-11: the flat `string After` was replaced by
    `Position`, an object. Omit it and the blocks go to the end, as before.

    abstract class ContentPosition { abstract string Type { get; } }
        new StartContentPosition()      // first among the parent's children
        new EndContentPosition()        // last (also the default)
        new AfterBlockContentPosition   // right after an existing sibling
            { AfterBlock = new AfterBlockReference { Id = siblingBlockId } }

    class BlockRetrieveChildrenRequest
        string BlockId; string StartCursor; int? PageSize

    await notion.Blocks.AppendChildrenAsync(new BlockAppendChildrenRequest
    {
        BlockId = pageId,
        Children = new List<IBlockObjectRequest>
        {
            new HeadingTwoBlockRequest
            {
                Heading_2 = new HeadingTwoBlockRequest.Info
                    { RichText = NotionText.PlainBase("Findings") },
            },
            new ParagraphBlockRequest
            {
                Paragraph = new ParagraphBlockRequest.Info
                    { RichText = NotionText.PlainBase("Body text.") },
            },
            new ToDoBlockRequest
            {
                ToDo = new ToDoBlockRequest.Info
                {
                    RichText = NotionText.PlainBase("Ship it"),
                    IsChecked = false,
                },
            },
            new CodeBlockRequest
            {
                Code = new CodeBlockRequest.Info
                {
                    RichText = NotionText.PlainBase("var x = 1;"),
                    Language = "csharp",
                },
            },
            new DividerBlockRequest { Divider = new DividerBlockRequest.Data() },
        },
    });

    // Update: send ONLY the block's own payload, in an ...UpdateBlock type.
    await notion.Blocks.UpdateAsync(blockId, new ParagraphUpdateBlock
    {
        Paragraph = new ParagraphUpdateBlock.Info
        {
            RichText = new List<RichTextBaseInput>
            {
                new RichTextTextInput
                    { Text = new Text { Content = "Revised text." } },
            },
        },
    });

    // Delete (move to trash):
    await notion.Blocks.DeleteAsync(blockId);

Update blocks take `RichTextBaseInput` (not `RichTextBase`) for their rich text
— `CodeUpdateBlock.Info.RichText` is the one exception and takes `RichTextBase`,
with `Caption` still `RichTextBaseInput`. `TableRowUpdateBlock.Info.Cells` is
`IEnumerable<IEnumerable<RichTextTextInput>>`, while
`TableRowBlockRequest.Info.Cells` is `IEnumerable<IEnumerable<RichTextText>>`.

SEARCH
======
    class SearchRequest : ISearchBodyParameters
        string       Query       { get; set; }   // matched against titles only
        SearchSort   Sort        { get; set; }
        SearchFilter Filter      { get; set; }
        string       StartCursor { get; set; }
        int?         PageSize    { get; set; }

    class SearchSort
        SearchDirection Direction { get; set; }   // Ascending | Descending
        string          Timestamp { get; set; }   // "last_edited_time"

    class SearchFilter
        SearchObjectType Value    { get; set; }   // Page | DataSource
        string           Property => "object"     // fixed

    class SearchResponse : PaginatedList<ISearchResponseObject>

`ISearchResponseObject : IObject` resolves to `Page` or `DataSource`.

    var results = await notion.Search.SearchAsync(new SearchRequest
    {
        Query = "quarterly",
        Filter = new SearchFilter { Value = SearchObjectType.Page },
        Sort = new SearchSort
        {
            Direction = SearchDirection.Descending,
            Timestamp = "last_edited_time",
        },
        PageSize = 50,
    });

    foreach (var page in results.Results.OfType<Page>())
    {
        Console.WriteLine(page.Url);
    }

Search only sees pages and data sources that have been SHARED with the
integration, and only matches titles.

USERS
=====
    var me = await notion.Users.MeAsync();          // the bot behind the token
    var user = await notion.Users.RetrieveAsync(userId);

    string cursor = null;
    do
    {
        var page = await notion.Users.ListAsync(
            new ListUsersRequest { StartCursor = cursor, PageSize = 100 });
        foreach (var u in page.Results) { Console.WriteLine(u.Name); }
        cursor = page.HasMore ? page.NextCursor : null;
    }
    while (cursor != null);

`MeAsync` returns a `User` whose `Type` is "bot" and whose `Bot.Owner` is a
`UserOwner` or `WorkspaceIntegrationOwner`.

COMMENTS
========
    class CreateCommentRequest : ICreateDiscussionCommentBodyParameters,
                                 ICreatePageCommentBodyParameters
        string DiscussionId; IEnumerable<RichTextBaseInput> RichText;
        ParentPageInput Parent
        static CreateCommentRequest CreatePageComment(
            ParentPageInput parent, IEnumerable<RichTextBaseInput> richText)
        static CreateCommentRequest CreateDiscussionComment(
            string discussionId, IEnumerable<RichTextBaseInput> richText)

    class ParentPageInput { string PageId }

    class RetrieveCommentsRequest : IRetrieveCommentsQueryParameters
        string BlockId; string StartCursor; int? PageSize

    class Comment : IObject
        IParentOfComment Parent; string DiscussionId;
        IEnumerable<RichTextBase> RichText; PartialUser CreatedBy;
        DateTime CreatedTime, LastEditedTime; string Id

    var comment = await notion.Comments.CreateAsync(
        CreateCommentRequest.CreatePageComment(
            new ParentPageInput { PageId = pageId },
            new List<RichTextBaseInput>
            {
                new RichTextTextInput
                    { Text = new Text { Content = "Looks good." } },
            }));

    var thread = await notion.Comments.RetrieveAsync(
        new RetrieveCommentsRequest { BlockId = pageId, PageSize = 100 });

Set `Parent` for a NEW discussion on a page, or `DiscussionId` to reply into an
existing one — never both.

Retrieve, update and delete a SINGLE comment (added in API 2026-03-11):

    class RetrieveSingleCommentRequest { string CommentId }
    class UpdateCommentRequest
        string CommentId                                  // path parameter
        IEnumerable<RichTextBase> RichText                // the new content

    var one = await notion.Comments.RetrieveSingleAsync(
        new RetrieveSingleCommentRequest { CommentId = comment.Id });

    var edited = await notion.Comments.UpdateAsync(new UpdateCommentRequest
    {
        CommentId = comment.Id,
        RichText = NotionText.PlainBase("Revised remark."),
    });

    await notion.Comments.DeleteAsync(comment.Id);

`UpdateCommentRequest.CommentId` is [JsonIgnore]d — it goes in the URL, not the
body. Update replaces the comment's rich text wholesale; there is no partial
edit. Delete is permanent and returns nothing.

VIEWS
=====
A VIEW is a saved presentation of a data source (table, board, calendar, ...).
The API models a view's rows through a two-step QUERY: create a query against
the view, then page through its results.

    class View : IObject
        string Id; ObjectType Object => ObjectType.View
        ViewType Type; string Name; string DataSourceId
        DatabaseParent Parent; DateTime CreatedTime, LastEditedTime
        string Url; PartialUser CreatedBy, LastEditedBy
        object Filter; IEnumerable<ViewSort> Sorts
        ViewConfiguration Configuration

    class ViewSort { string Property, Timestamp, Direction }
    class PageReference { string Object, Id }

    class ListViewsRequest
        string DatabaseId, DataSourceId, StartCursor; int? PageSize
    class CreateViewRequest
        string DataSourceId, Name; ViewType Type; string DatabaseId
        object Filter; IEnumerable<ViewSort> Sorts
        ViewConfiguration Configuration
    class UpdateViewRequest
        string ViewId                                     // path parameter
        string Name; object Filter
        IEnumerable<UpdateViewSort> Sorts                 // { Property, Direction }
        ViewConfiguration Configuration
    class CreateViewQueryRequest      { string ViewId; int? PageSize }
    class GetViewQueryResultsRequest  { string ViewId, QueryId, StartCursor;
                                        int? PageSize }
    class DeleteViewQueryRequest      { string ViewId, QueryId }
    class ViewQueryResponse
        string Object, Id, ViewId; DateTime ExpiresAt; int TotalCount
        IEnumerable<PageReference> Results; string NextCursor; bool HasMore
    class DeletedViewQueryResponse    { string Object, Id; bool Deleted }

`ViewType` (extensible string-enum struct): `Table`, `Board`, `List`,
`Calendar`, `Timeline`, `Gallery`, `Form`, `Chart`, `Map`, `Dashboard`.

`ViewConfiguration` is an ABSTRACT base discriminated on "type", with one
concrete subclass per view type — `TableViewConfiguration`,
`BoardViewConfiguration`, `CalendarViewConfiguration`,
`TimelineViewConfiguration`, `GalleryViewConfiguration`,
`ListViewConfiguration`, `MapViewConfiguration`, `FormViewConfiguration`,
`ChartViewConfiguration`, `DashboardViewConfiguration` — plus
`UnknownViewConfiguration` as the fallback for a view type Notion adds later.
Each one holds its per-type settings in `IDictionary<string, object>
AdditionalData`, so nothing is lost even though the shapes are not modelled.

    // list, then read one in full
    var views = await notion.Views.ListAsync(
        new ListViewsRequest { DataSourceId = dataSourceId });
    var view = await notion.Views.RetrieveAsync(views.Results[0].Id);

    // query a view's rows
    var query = await notion.Views.CreateQueryAsync(
        new CreateViewQueryRequest { ViewId = view.Id, PageSize = 100 });
    var rows = await notion.Views.GetQueryResultsAsync(
        new GetViewQueryResultsRequest
            { ViewId = view.Id, QueryId = query.Id, PageSize = 100 });
    await notion.Views.DeleteQueryAsync(
        new DeleteViewQueryRequest { ViewId = view.Id, QueryId = query.Id });

GOTCHAS, all confirmed against the live API:
  * ListAsync returns PARTIAL views — only `Object` and `Id` are populated.
    Call RetrieveAsync for `Name`, `Type`, `Sorts` and `Configuration`.
  * CreateAsync needs BOTH `DatabaseId` AND `DataSourceId`. Supplying only one
    fails with a validation_error naming the other.
  * DeleteAsync refuses to remove a database's LAST view
    ("Cannot delete the last view of a database. Delete the database instead.").
  * A view query is a SNAPSHOT with an `ExpiresAt`; re-create it rather than
    holding a query id for long.

CUSTOM EMOJIS
=============
    class ListEmojisRequest : IListEmojisQueryParameters
        string StartCursor; int? PageSize
    class ListEmojisResponse : PaginatedList<CustomEmoji>
        Dictionary<string, object> CustomEmoji
    class CustomEmoji { string Id, Name, Url;
                        IDictionary<string, object> AdditionalData }

    var emojis = await notion.Emojis.ListAsync(
        new ListEmojisRequest { PageSize = 100 });

The ids returned here are what `CustomEmojiPageIconRequest` wants when you set
a custom emoji as a page icon.

MEETING NOTES
=============
    class QueryMeetingNotesRequest
        object Filter; object Sort; int? Limit
        string StartCursor; int? PageSize
    class QueryMeetingNotesResponse : PaginatedList<MeetingNotesBlock>

    var notes = await notion.Blocks.QueryMeetingNotesAsync(
        new QueryMeetingNotesRequest { PageSize = 100 });

`Filter` and `Sort` are `object` because Notion has not published their shapes;
pass an anonymous object or a Dictionary. The endpoint requires a Notion plan
with AI meeting notes enabled and otherwise answers
"This endpoint requires a plan with AI meeting notes enabled."

FILE UPLOADS
============
    class CreateFileUploadRequest
        FileUploadMode Mode      // SinglePart | MultiPart | ExternalUrl
        string FileName, ContentType
        int? NumberOfParts       // multi-part only
        string ExternalUrl       // external-url mode only
    class SendFileUploadRequest
        static SendFileUploadRequest Create(
            string fileUploadId, FileData file, string partNumber = null)
        FileData File { get; }  string PartNumber { get; }
        string FileUploadId { get; }
    class FileData { string FileName; Stream Data; string ContentType }
    class CompleteFileUploadRequest { string FileUploadId }
    class ListFileUploadsRequest { string Status; string StartCursor;
                                   int? PageSize }
    class RetrieveFileUploadRequest { string FileUploadId }

    class FileUpload : IObject
        string Id, Status, FileName, ContentType, UploadUrl, CompleteUrl
        long? ContentLength; DateTime? ExpiryTime; bool Archived
        DateTime CreatedTime, LastEditedTime; PartialUser CreatedBy
        FileImportResult FileImportResult
    abstract class FileImportResult -> FileImportSuccessResult |
        FileImportErrorResult (FileImportError { Type, Code, Message,
        Parameter, StatusCode }) | UnknownFileImportResult

Single-part flow: `CreateAsync` -> `SendAsync` -> use the returned id.
Multi-part flow: `CreateAsync(Mode = MultiPart, NumberOfParts = n)` ->
`SendAsync` once per part with `partNumber` -> `CompleteAsync`. Then reference
the id from a block or icon:

    var upload = await notion.FileUploads.CreateAsync(new CreateFileUploadRequest
    {
        Mode = FileUploadMode.SinglePart,
        FileName = "diagram.png",
        ContentType = "image/png",
    });

    using (var stream = File.OpenRead("/path/diagram.png"))
    {
        await notion.FileUploads.SendAsync(SendFileUploadRequest.Create(
            upload.Id,
            new FileData
            {
                FileName = "diagram.png",
                Data = stream,
                ContentType = "image/png",
            }));
    }

    var imageBlock = NotionBlocks.ImageUpload(upload.Id);

For the common single-part case, `UploadFileAsync` (below) does all of this in
one call.

OAUTH AUTHENTICATION
====================
Only needed for public integrations. `CreateTokenRequest` (with
`ExternalAccount`), `RefreshTokenRequest`, `IntrospectTokenRequest` and
`RevokeTokenRequest` map onto the four `IAuthenticationClient` methods and
return `CreateTokenResponse`, `RefreshTokenResponse` (with `Owner`) and
`IntrospectTokenResponse`. The client-id/client-secret pair is supplied
through `IBasicAuthenticationParameters`. A private integration token needs
none of this — just set `ClientOptions.AuthToken`.

AUTHORING HELPERS (start here for building pages)
=================================================
For the common "build a page from local content" workflow, prefer these helpers
over hand-writing request objects. They smooth over Notion's hard limits and
the raw nested-`.Info` ceremony, and they are the recommended path.

    static class NotionText
        const int MaxRunLength = 2000
        static RichTextText Run(string content, bool bold = false,
            bool italic = false, bool code = false, string linkUrl = null)
        static List<RichTextText> Split(string content, bool bold = false,
            bool italic = false, bool code = false, string linkUrl = null)
        static List<RichTextBase> SplitBase(string content, bool bold = false,
            bool italic = false, bool code = false, string linkUrl = null)
        static List<RichTextText> Plain(string text)        // == Split(text)
        static List<RichTextBase> PlainBase(string text)    // == SplitBase(text)

    Notion REJECTS any rich-text run longer than 2,000 characters. Split /
    SplitBase break long text at WORD boundaries into legal runs; use them
    (or Plain / PlainBase, which call them) for anything not known to be short.

    static class NotionBlocks
        static ParagraphBlockRequest Paragraph(
            IEnumerable<RichTextBase> richText, Color? color = null)
        static ParagraphBlockRequest Paragraph(string text, Color? color = null)
        static HeadingTwoBlockRequest   Heading2(string text)
        static HeadingThreeBlockRequest Heading3(string text)
        static CalloutBlockRequest Callout(IEnumerable<RichTextBase> richText,
            string emoji = null,
            IEnumerable<INonColumnBlockRequest> children = null,
            Color? color = null)
        static CalloutBlockRequest Callout(string text, string emoji = null,
            Color? color = null)
        static QuoteBlockRequest Quote(IEnumerable<RichTextBase> richText)
        static QuoteBlockRequest Quote(string text)
        static BulletedListItemBlockRequest Bullet(string text)
        static NumberedListItemBlockRequest Numbered(string text)
        static ToggleBlockRequest Toggle(IEnumerable<RichTextBase> summary,
            IEnumerable<INonColumnBlockRequest> children)
        static ToggleBlockRequest Toggle(string summary,
            IEnumerable<INonColumnBlockRequest> children)
        static DividerBlockRequest Divider()
        static TableRowBlockRequest TableRow(
            IEnumerable<IEnumerable<RichTextText>> cells)
        static TableBlockRequest Table(
            IReadOnlyList<IReadOnlyList<IEnumerable<RichTextText>>> rows,
            bool hasColumnHeader = false, bool hasRowHeader = false)
        static ImageBlockRequest ImageExternal(string url,
            IEnumerable<RichTextBase> caption = null)
        static ImageBlockRequest ImageUpload(string fileUploadId,
            IEnumerable<RichTextBase> caption = null)

    There is no Heading1 factory — h1 is reserved for the page title in most
    documents; build one with `new HeadingOneBlockRequest { Heading_1 = ... }`
    if you really want it. The string overloads treat their argument as LITERAL
    text — no markdown is parsed. Build emphasis explicitly with NotionText.Run.
    `Table` takes the table width from the first row.

    static class NotionAuthoringExtensions   // extension methods on INotionClient
        const int MaxChildrenPerAppend = 100
        static Task<Page> CreateChildPageAsync(this INotionClient client,
            string parentPageId, string title,
            CancellationToken cancellationToken = default)
        static Task AppendChildrenBatchedAsync(this INotionClient client,
            string parentBlockId, IReadOnlyList<IBlockObjectRequest> blocks,
            int throttleMs = 350, CancellationToken cancellationToken = default)
        static Task<string> UploadFileAsync(this INotionClient client,
            string filePath, string contentType = null,
            CancellationToken cancellationToken = default)
        static Task<Page> ArchivePageAsync(this INotionClient client,
            string pageId, CancellationToken cancellationToken = default)
        static Task<List<IBlock>> RetrieveAllChildrenAsync(
            this INotionClient client, string blockId,
            CancellationToken cancellationToken = default)
        static string GuessContentType(string path)

    AppendChildrenBatchedAsync appends ANY number of blocks, splitting into
    sequential requests of at most 100 (Notion's per-request cap) so order is
    preserved, pausing `throttleMs` between batches (default 350 ms, i.e.
    roughly 3 requests per second). Pass 0 to disable.
    UploadFileAsync performs the single-part Create+Send in one call and
    returns the `file_upload` id. RetrieveAllChildrenAsync follows pagination
    to the end with PageSize 100. ArchivePageAsync sets `in_trash = true`.

Blessed end-to-end pattern (create with a title, then append the body):

    var page = await notion.CreateChildPageAsync(parentPageId, "My Page");

    var uploadId = await notion.UploadFileAsync("/path/diagram.png");
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
    await notion.AppendChildrenBatchedAsync(page.Id, blocks);

MARKDOWN ROUND-TRIP
===================
Notion ingests and emits its own flavour of markdown, which is often simpler
than building blocks when you do not need fine interleaving.

    // CREATE a page from markdown
    var created = await notion.Pages.CreateAsync(new PagesCreateParameters
    {
        Parent = new PageParentRequest { PageId = parentPageId },
        Properties = new Dictionary<string, PropertyValue>
        {
            ["title"] = new TitlePropertyValue
                { Title = NotionText.PlainBase("Imported notes") },
        },
        Markdown = "# Heading\n\nSome **bold** text.\n",
    });

    // READ a page back as markdown
    var md = await notion.Pages.RetrieveAsMarkdownAsync(
        new RetrievePageAsMarkdownRequest
            { PageId = created.Id, IncludeTranscript = false });
    Console.WriteLine(md.Markdown);        // md.Truncated, md.UnknownBlockIds

    // UPDATE page content with markdown — four operations:
    await notion.Pages.UpdateMarkdownAsync(created.Id,
        new InsertContentMarkdownBody
        {
            InsertContent = new InsertContentData
            {
                Content = "\n## Appendix\n",
                Position = new EndMarkdownInsertPosition(),
            },
        });

    abstract class UpdatePageMarkdownBody { abstract string Type { get; } }
      InsertContentMarkdownBody      .InsertContent -> InsertContentData
          { string Content; string After; MarkdownInsertPosition Position }
          MarkdownInsertPosition -> StartMarkdownInsertPosition |
                                    EndMarkdownInsertPosition
          (`After` is an ellipsis-format selection such as
           "start text...end text"; it cannot be combined with `Position`.)
      ReplaceContentRangeMarkdownBody .ReplaceContentRange ->
          ReplaceContentRangeData { string Content; string ContentRange;
                                    bool? AllowDeletingContent }
      UpdateContentMarkdownBody       .UpdateContent -> UpdateContentData
          { IList<ContentUpdate> ContentUpdates; bool? AllowDeletingContent }
          ContentUpdate { string OldStr; string NewStr;
                          bool? ReplaceAllMatches }
      ReplaceContentMarkdownBody      .ReplaceContent -> ReplaceContentData
          { string NewStr; bool? AllowDeletingContent }

    class PageMarkdownResponse : IObject
        string Markdown; bool Truncated; IEnumerable<string> UnknownBlockIds

`PagesCreateParametersBuilder` offers the same thing fluently:

    var parameters = PagesCreateParametersBuilder
        .Create(new PageParentRequest { PageId = parentPageId })
        .AddProperty("title", new TitlePropertyValue
            { Title = NotionText.PlainBase("Imported notes") })
        .SetMarkdown("# Heading\n\nBody.\n")
        .SetIcon(new EmojiPageIconRequest { Emoji = "\U0001F4C4" })
        .SetPosition(new PageStartPosition())
        .Build();

    (Create, AddProperty, AddPageContent, SetIcon, SetCover, SetMarkdown,
     SetTemplate, SetPosition, Build.)

PAGE AND BLOCK ORDERING — a Notion gotcha
=========================================
Notion keeps a page's children (sub-pages AND content blocks) in INSERTION
order, and its API offers NO operation to reposition an existing child in
place. This is a Notion API constraint, not a limitation of this library — the
library models the endpoints faithfully.

What you CANNOT do:
  * Reorder existing sibling pages/blocks. There is no "reorder" endpoint.
  * Reposition with Move. `Pages.MoveAsync` (`MovePageRequest { string PageId;
    MovePageParent Parent }`, where `MovePageParent { string Type; string
    DatabaseId; string PageId }`) accepts ONLY a new parent — the endpoint has
    no position parameter. It re-parents a page; it cannot change a page's
    order under the same parent.

What you CAN do — positional control exists only at INSERT time:
  * Create a page at a chosen slot. Set `PagesCreateParameters.Position` to a
    `PagePosition` subtype:
        new PageStartPosition()               // first under the parent
        new PageEndPosition()                 // last (also the default)
        new AfterBlockPagePosition            // right after a sibling
            { AfterBlock = new AfterBlockReference { Id = siblingBlockId } }
  * Append blocks at a chosen slot. Set `BlockAppendChildrenRequest.Position`
    to a `ContentPosition` subtype — `StartContentPosition`,
    `EndContentPosition`, or `AfterBlockContentPosition { AfterBlock =
    new AfterBlockReference { Id = siblingBlockId } }`. With no `Position`,
    appended blocks go to the end.

Consequence / recipe: because there is no in-place reorder, get ordering right
by CREATING in the desired order (optionally via Position). To fix the order of
pages that already exist, trash them
(`Pages.UpdateAsync` with `PagesUpdateParameters.InTrash = true`, or
`ArchivePageAsync`) and recreate them in sequence.

Page create/update parameter reference
--------------------------------------
    class PagesCreateParameters
        IParentOfPageRequest                 Parent
        IDictionary<string, PropertyValue>   Properties
        IEnumerable<IBlockObjectRequest>     Children
        IPageIconRequest                     Icon
        IPageCoverRequest                    Cover
        string                               Markdown
        PageTemplate                         Template
        PagePosition                         Position

    class PagesUpdateParameters
        IPageIconRequest                     Icon
        IPageCoverRequest                    Cover
        bool                                 InTrash
        IDictionary<string, PropertyValue>   Properties
        PageTemplate                         Template
        bool?                                EraseContent
        bool?                                IsLocked

    abstract class PageTemplate -> NonePageTemplate
                                 | DefaultPageTemplate { string Timezone }
                                 | TemplateIdPageTemplate
                                     { string TemplateId; string Timezone }

    class Page : IObject, IObjectModificationData,
                 IQueryDataSourceResponseObject, ISearchResponseObject
        IParentOfPage Parent; bool InTrash; bool? IsLocked;
        IDictionary<string, PropertyValue> Properties;
        string Url, PublicUrl; IPageIcon Icon; IPageCover Cover
        (plus Id, CreatedTime, LastEditedTime, CreatedBy, LastEditedBy)

POLYMORPHIC DESERIALIZATION (from the CodeBrix.Json.Extensions dependency)
=========================================================================
Every "one of N shapes" model in this library — IBlock, PropertyValue,
IPropertyItemObject, RichTextBase, FileObject, the parent interfaces, IPageIcon,
IPageCover, DataSourcePropertyConfig, Property, IObject — is annotated with
attributes from the `CodeBrix.Json.Extensions.Polymorphism` namespace, supplied
by the CodeBrix.Json.Extensions.MitLicenseForever package. You only need these
types if you declare your OWN polymorphic hierarchy:

    [JsonConverter(typeof(FallbackTypeConverterFactory))]
    [JsonDiscriminator("type")]
    [JsonKnownType(typeof(ParagraphBlock), BlockType.ParagraphValue)]
    [JsonFallbackType(typeof(UnsupportedBlock))]
    public interface IMyThing { }

    JsonDiscriminatorAttribute(string propertyName)
    JsonKnownTypeAttribute(Type knownType, string discriminatorValue)  // multiple
    JsonFallbackTypeAttribute(Type fallbackType)
    FallbackTypeConverter<T>, FallbackTypeConverterFactory

Rules enforced at runtime (InvalidOperationException): known/fallback targets
must be assignable to the base type; a type may not declare ITSELF as a
known/fallback target (which is why this library ships derived `Unknown*`
classes — UnknownRichText, UnknownPropertyValue, UnknownProperty,
UnknownPropertyItem, UnknownFileObject, UnknownFileObjectWithName,
UnknownFileImportResult, UnknownDataSourcePropertyConfig, UnknownObject);
targets must be instantiable or themselves declare `[JsonDiscriminator]`
(two-level chains are supported); duplicate discriminator values are rejected.

The practical consequence for you: unknown values coming back from Notion do
NOT throw. A new block type deserializes as `UnsupportedBlock`, a new property
type as `UnknownPropertyValue`, and so on. Always handle those cases.

COMPLETE EXAMPLES
=================
1 — Create a page in a data source, with properties, icon, cover and content
----------------------------------------------------------------------------
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodeBrix.NotionApi;

    using var notion = NotionClientFactory.Instance.Create(new ClientOptions
    {
        AuthToken = Environment.GetEnvironmentVariable("NOTION_AUTH_TOKEN"),
        RetryPolicy = new DefaultRetryPolicy(),
    });

    var database = await notion.Databases.RetrieveAsync(databaseId);
    var dataSourceId = database.DataSources.First().DataSourceId;

    var page = await notion.Pages.CreateAsync(new PagesCreateParameters
    {
        Parent = new DataSourceParentRequest { DataSourceId = dataSourceId },
        Properties = new Dictionary<string, PropertyValue>
        {
            ["Name"] = new TitlePropertyValue
                { Title = NotionText.PlainBase("Quarterly report") },
            ["Stage"] = new SelectPropertyValue
                { Select = new SelectOption { Name = "Review" } },
            ["Tags"] = new MultiSelectPropertyValue
            {
                MultiSelect = new List<SelectOption>
                {
                    new SelectOption { Name = "finance" },
                    new SelectOption { Name = "q3" },
                },
            },
            ["Due"] = new DatePropertyValue
            {
                Date = new Date
                {
                    Start = DateTimeOffset.UtcNow.AddDays(14),
                    IncludeTime = false,
                },
            },
            ["Points"] = new NumberPropertyValue { Number = 8 },
            ["Done"] = new CheckboxPropertyValue { Checkbox = false },
            ["Docs"] = new UrlPropertyValue { Url = "https://example.com/q3" },
        },
        Icon = new EmojiPageIconRequest { Emoji = "\U0001F4C8" },
        Cover = new ExternalPageCoverRequest
        {
            External = new ExternalPageCoverRequest.Info
                { Url = "https://example.com/cover.png" },
        },
        Children = new List<IBlockObjectRequest>
        {
            NotionBlocks.Heading2("Summary"),
            NotionBlocks.Paragraph("Revenue was up."),
        },
    });

    Console.WriteLine(page.Url);

2 — Query a data source with a compound filter, sorts and pagination
--------------------------------------------------------------------
    var filter = new CompoundFilter(and: new List<Filter>
    {
        new SelectFilter("Stage", equal: "Review"),
        new CheckboxFilter("Done", equal: false),
        new DateFilter("Due", onOrBefore: DateTime.UtcNow.AddDays(30)),
        new CompoundFilter(or: new List<Filter>
        {
            new MultiSelectFilter("Tags", contains: "finance"),
            new RichTextFilter("Notes", contains: "budget"),
        }),
    });

    var matches = new List<Page>();
    string cursor = null;
    do
    {
        var response = await notion.DataSources.QueryAsync(
            new QueryDataSourceRequest
            {
                DataSourceId = dataSourceId,
                Filter = filter,
                Sorts = new List<Sort>
                {
                    new Sort
                    {
                        Property = "Due",
                        Direction = Direction.Ascending,
                    },
                    new Sort
                    {
                        Timestamp = Timestamp.LastEditedTime,
                        Direction = Direction.Descending,
                    },
                },
                PageSize = 100,
                StartCursor = cursor,
            });

        matches.AddRange(response.Results.OfType<Page>());
        cursor = response.HasMore ? response.NextCursor : null;
    }
    while (cursor != null);

    Console.WriteLine($"{matches.Count} matching pages");

3 — Read every property of a page
----------------------------------
    var full = await notion.Pages.RetrieveAsync(page.Id);

    foreach (var entry in full.Properties)
    {
        var text = entry.Value switch
        {
            TitlePropertyValue t =>
                string.Concat(t.Title.Select(r => r.PlainText)),
            RichTextPropertyValue r =>
                string.Concat(r.RichText.Select(x => x.PlainText)),
            SelectPropertyValue s      => s.Select?.Name,
            MultiSelectPropertyValue m =>
                string.Join(", ", m.MultiSelect.Select(o => o.Name)),
            StatusPropertyValue st     => st.Status?.Name,
            DatePropertyValue d        => d.Date?.Start?.ToString("d"),
            NumberPropertyValue n      => n.Number?.ToString(),
            CheckboxPropertyValue c    => c.Checkbox.ToString(),
            UrlPropertyValue u         => u.Url,
            EmailPropertyValue e       => e.Email,
            PeoplePropertyValue p      =>
                string.Join(", ", p.People.Select(u => u.Name)),
            RelationPropertyValue rel  =>
                string.Join(", ", rel.Relation.Select(o => o.Id)),
            FormulaPropertyValue f     => f.Formula?.String,
            UnknownPropertyValue       => "(unsupported property type)",
            _                          => entry.Value.Type.ToString(),
        };

        Console.WriteLine($"{entry.Key}: {text}");
    }

4 — Append, update and delete blocks
-------------------------------------
    var appended = await notion.Blocks.AppendChildrenAsync(
        new BlockAppendChildrenRequest
        {
            BlockId = page.Id,
            Children = new List<IBlockObjectRequest>
            {
                NotionBlocks.Heading2("Findings"),
                NotionBlocks.Bullet("Costs fell."),
                NotionBlocks.Bullet("Headcount flat."),
                NotionBlocks.Quote("Steady as she goes."),
                NotionBlocks.Divider(),
                NotionBlocks.Table(
                    new List<IReadOnlyList<IEnumerable<RichTextText>>>
                    {
                        new List<IEnumerable<RichTextText>>
                        {
                            NotionText.Plain("Metric"),
                            NotionText.Plain("Value"),
                        },
                        new List<IEnumerable<RichTextText>>
                        {
                            NotionText.Plain("Revenue"),
                            NotionText.Plain("1.2M"),
                        },
                    },
                    hasColumnHeader: true),
            },
        });

    var firstBullet = appended.Results
        .OfType<BulletedListItemBlock>()
        .First();

    await notion.Blocks.UpdateAsync(firstBullet.Id,
        new BulletedListItemUpdateBlock
        {
            BulletedListItem = new BulletedListItemUpdateBlock.Info
            {
                RichText = new List<RichTextBaseInput>
                {
                    new RichTextTextInput
                        { Text = new Text { Content = "Costs fell 12%." } },
                },
            },
        });

    await notion.Blocks.DeleteAsync(firstBullet.Id);

    // Read the whole subtree back, following pagination:
    var children = await notion.RetrieveAllChildrenAsync(page.Id);

5 — Search, then read one result as markdown
---------------------------------------------
    var found = await notion.Search.SearchAsync(new SearchRequest
    {
        Query = "Quarterly",
        Filter = new SearchFilter { Value = SearchObjectType.Page },
        Sort = new SearchSort
        {
            Direction = SearchDirection.Descending,
            Timestamp = "last_edited_time",
        },
        PageSize = 25,
    });

    var hit = found.Results.OfType<Page>().FirstOrDefault();
    if (hit != null)
    {
        var markdown = await notion.Pages.RetrieveAsMarkdownAsync(
            new RetrievePageAsMarkdownRequest { PageId = hit.Id });
        Console.WriteLine(markdown.Markdown);
    }

6 — Markdown round trip
------------------------
    var imported = await notion.Pages.CreateAsync(new PagesCreateParameters
    {
        Parent = new PageParentRequest { PageId = parentPageId },
        Properties = new Dictionary<string, PropertyValue>
        {
            ["title"] = new TitlePropertyValue
                { Title = NotionText.PlainBase("Imported notes") },
        },
        Markdown = "# Notes\n\nFirst **paragraph**.\n\n- one\n- two\n",
    });

    await notion.Pages.UpdateMarkdownAsync(imported.Id,
        new InsertContentMarkdownBody
        {
            InsertContent = new InsertContentData
            {
                Content = "\n## Appendix\n\nAdded later.\n",
                Position = new EndMarkdownInsertPosition(),
            },
        });

    var roundTripped = await notion.Pages.RetrieveAsMarkdownAsync(
        new RetrievePageAsMarkdownRequest { PageId = imported.Id });

    Console.WriteLine(roundTripped.Markdown);
    if (roundTripped.Truncated)
    {
        Console.WriteLine("Content was truncated by Notion.");
    }

MINIMUM VIABLE PROJECT
======================
MyNotionTool.csproj

    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net10.0</TargetFramework>
        <Nullable>disable</Nullable>
      </PropertyGroup>
      <ItemGroup>
        <PackageReference Include="CodeBrix.NotionApi.MitLicenseForever" />
      </ItemGroup>
    </Project>

(Add the version attribute your project's policy requires, or manage it with
Central Package Management.)

Program.cs

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CodeBrix.NotionApi;

    internal static class Program
    {
        private static async Task Main()
        {
            var token = Environment.GetEnvironmentVariable("NOTION_AUTH_TOKEN");
            var parentPageId =
                Environment.GetEnvironmentVariable("NOTION_PARENT_PAGE_ID");

            using var notion = NotionClientFactory.Instance.Create(
                new ClientOptions
                {
                    AuthToken = token,
                    RetryPolicy = new DefaultRetryPolicy(),
                });

            var me = await notion.Users.MeAsync();
            Console.WriteLine($"Connected as: {me.Name}");

            var page = await notion.CreateChildPageAsync(
                parentPageId, "Hello from CodeBrix.NotionApi");

            await notion.AppendChildrenBatchedAsync(page.Id,
                new List<IBlockObjectRequest>
                {
                    NotionBlocks.Heading2("It works"),
                    NotionBlocks.Paragraph("Created by a .NET console app."),
                });

            Console.WriteLine(page.Url);
        }
    }

Share the parent page with your integration in Notion first, or every call
fails with `NotionAPIErrorCode.ObjectNotFound`.

For an ASP.NET Core host, replace the factory line with
`services.AddNotionClientFactory();` and inject `INotionClientFactory` (see
"DEPENDENCY INJECTION").

PERFORMANCE TIPS
================
  * Pool HttpClient. In DI, use `AddNotionClientFactory()` and create one
    INotionClient per unit of work; outside DI, reuse ONE long-lived client (or
    give NotionClientFactory.Instance an IHttpClientFactory via
    `SetHttpClientFactory`). Never construct a client per request without a
    factory — each one owns a fresh connection pool and leaks sockets.
  * Batch appends. `AppendChildrenBatchedAsync` sends at most 100 children per
    request; one 100-block request is far cheaper than 100 one-block requests.
  * Pace writes. Keep `throttleMs` at its default (350 ms, about 3 requests per
    second) for bulk authoring; drop it only when you have measured headroom,
    and set `RetryPolicy = new DefaultRetryPolicy()` so 429s are absorbed with
    the server-supplied Retry-After.
  * Ask for full pages. `PageSize = 100` is the maximum everywhere; leaving it
    unset makes Notion pick a smaller default and triples your round trips.
  * Narrow the payload. `QueryDataSourceRequest.FilterProperties` is the
    endpoint's list of properties to filter the results by; setting it keeps
    large query responses smaller.
  * Filter server-side. Push conditions into `Filter`/`Sorts` instead of
    fetching everything and filtering in memory — the query endpoint is
    paginated, so client-side filtering costs whole extra round trips.
  * Do not double-retry. If your HttpClient pipeline already retries (e.g.
    Polly registered through IHttpClientFactory), leave
    `ClientOptions.RetryPolicy` null.
  * Reuse one client across many calls in a unit of work; the per-call cost of
    `Create` is small but the connection pool behind it is what matters.

COMMON PITFALLS TO AVOID
========================
  * Calling `Databases.QueryAsync` — it does not exist. Query
    `DataSources.QueryAsync` with a `DataSourceId` taken from
    `Database.DataSources`.
  * Writing a rich-text run longer than 2,000 characters. Notion rejects the
    whole request. Use `NotionText.Split` / `PlainBase`.
  * Appending more than 100 children in one request. Use
    `AppendChildrenBatchedAsync`, or chunk it yourself.
  * Forgetting to share the target page/database with the integration. Every
    call then fails with `object_not_found`, which reads like a wrong id.
  * Leaving `Direction` or `Timestamp` unset on a `Sort`. Their default value
    is `Unknown`, not `Ascending` / `CreatedTime`.
  * Mixing the request and response families: `RichTextBase` vs
    `RichTextBaseInput`, `IPageIcon` vs `IPageIconRequest`, `IParentOfPage` vs
    `IParentOfPageRequest`, `DataSourcePropertyConfig` vs
    `DataSourcePropertyConfigRequest`, `Block` vs `BlockRequest` vs
    `UpdateBlock`. The compiler catches most of these; the parent families are
    the ones people get wrong.
  * Sending a full block to `Blocks.UpdateAsync`. It takes an `IUpdateBlock`
    carrying only the changed payload, not the `...BlockRequest` type.
  * Expecting to reorder existing children. There is no reorder endpoint; see
    "PAGE AND BLOCK ORDERING".
  * Expecting `MoveAsync` to reposition. It only re-parents.
  * Disposing an INotionClient that came from `AddNotionClient` DI
    registration — the container owns it. DO dispose the ones you get from
    `INotionClientFactory.Create`.
  * Calling `SetHttpClientFactory` twice, or after the factory has created a
    client — both throw `InvalidOperationException`.
  * Serializing these models with your own default `JsonSerializerOptions`.
    Abstract/interface-typed members need the library's
    `RuntimeTypeConverterFactory` to write the runtime type.
  * Assuming an unknown type will throw. It will not — you will silently get
    `UnsupportedBlock`, `UnknownPropertyValue`, `UnknownRichText`,
    `UnknownObject`, etc. Handle them.
  * Treating `Search` as full-text search. It matches titles only, and only
    over content shared with the integration.
  * Setting both `Parent` and `DiscussionId` on a `CreateCommentRequest`, or
    both `After` and `Position` on `InsertContentData`.
  * Hard-coding a `NotionVersion`. The default (2026-03-11) is what these
    models are shaped for; changing it changes request/response shapes.
  * Still setting `BlockAppendChildrenRequest.After`. It is gone — use
    `Position` with a `ContentPosition` subtype.
  * Reading `Archived` on a page, database, data source or file upload. It is
    [Obsolete] and Notion no longer returns `archived` on a page; use
    `InTrash`.
  * Sending `list_start_index` or `list_format` on a numbered list item, or an
    `Icon` on a paragraph that is not a direct child of a tab block. Both are
    validation errors.

WHAT THIS PACKAGE DOES NOT DO
=============================
  * It does not reorder existing pages or blocks — Notion has no such endpoint
    (positional control exists only at insert time).
  * It does not reposition on move — `Pages.MoveAsync` re-parents only.
  * It does not reorder blocks after the fact — `BlockAppendChildrenRequest`
    now carries a `Position` object (start / end / after_block), but that is
    still INSERT-time control only.
  * It does not delete pages. Trash them
    (`PagesUpdateParameters.InTrash = true`).
  * It does not parse or render markdown itself; the markdown endpoints hand
    strings to and from Notion, which does the conversion server-side.
  * It does not cache, memoize or dedupe requests, and it does not rate-limit
    proactively — only `AppendChildrenBatchedAsync` paces itself, and retry is
    opt-in.
  * It does not manage OAuth flows beyond the four token endpoints (no browser
    redirect handling, no token storage).
  * It does not provide LINQ-over-Notion, an ORM, or change tracking; you build
    request objects and read response objects.
  * It does not ship the polymorphic-JSON attributes themselves — those come
    from the CodeBrix.Json.Extensions.MitLicenseForever dependency.
  * It does not target anything below .NET 10, and it does not multi-target.

WORKING EXAMPLES ON GITHUB
==========================
The test suite is the most complete worked example set:

  https://github.com/ellisnet/CodeBrix.NotionApi/tree/main/tests/CodeBrix.NotionApi.Tests

  PagesClientTests.cs        create / retrieve / update pages and properties
  BlocksClientTests.cs       retrieve, append, update and delete blocks
  DataSourcesClientTests.cs  retrieve / create / update / query data sources
  FilterTests.cs             every filter type, with the exact JSON produced
  PropertyTests.cs           property-value serialization round trips
  SearchClientTests.cs       search requests and responses
  UserClientTests.cs         Me / Retrieve / List
  FileUploadsClientTests.cs  the create-send-complete upload flow
  AuthenticationClientTests.cs   OAuth token endpoints
  RetryPolicyTests.cs        DefaultRetryPolicy behaviour on 429 / 5xx
  NotionClientFactoryTests.cs    factory and IHttpClientFactory wiring
  Authoring/                 NotionText, NotionBlocks and the INotionClient
                             authoring extensions
  Models/                    block-request and page-icon deserialization
  Integration/               end-to-end tests against the real Notion API —
                             read these for realistic multi-call sequences

QUICK REFERENCE CARD
====================
    PackageId .............. CodeBrix.NotionApi.MitLicenseForever
    Namespace .............. CodeBrix.NotionApi        (single, flat)
    License ................ MIT
    Target ................. .NET 10 or later
    API version sent ....... Notion-Version: 2026-03-11

    Create client .......... NotionClientFactory.Instance.Create(
                                 new ClientOptions { AuthToken = "ntn_..." })
    DI (recommended) ....... services.AddNotionClientFactory();
                             inject INotionClientFactory, Create per unit of
                             work, dispose it
    DI (single client) ..... services.AddNotionClient(o => o.AuthToken = ...);
                             inject INotionClient, do NOT dispose

    Sub-clients ............ Users, Pages, Databases, DataSources, Views,
                             Blocks, Search, Comments, Emojis, FileUploads,
                             AuthenticationClient, RestClient

    Create page ............ Pages.CreateAsync(PagesCreateParameters)
    Read page .............. Pages.RetrieveAsync(pageId)
    Write properties ....... Pages.UpdatePropertiesAsync(
                                 pageId, IDictionary<string, PropertyValue>)
    Trash page ............. Pages.UpdateAsync(pageId,
                                 new PagesUpdateParameters { InTrash = true })
    Big property ........... Pages.RetrievePagePropertyItemAsync(...)
    Page as markdown ....... Pages.RetrieveAsMarkdownAsync(...)
    Edit via markdown ...... Pages.UpdateMarkdownAsync(pageId, body)
    Re-parent page ......... Pages.MoveAsync(MovePageRequest)

    Query rows ............. DataSources.QueryAsync(QueryDataSourceRequest)
                             (NOT Databases.QueryAsync — it does not exist)
    Database -> data source  Databases.RetrieveAsync(id)
                                 .DataSources.First().DataSourceId

    Blocks ................. Blocks.RetrieveAsync / RetrieveChildrenAsync /
                             AppendChildrenAsync / UpdateAsync / DeleteAsync
    Search ................. Search.SearchAsync(SearchRequest)
    Users .................. Users.MeAsync / RetrieveAsync / ListAsync
    Comments ............... Comments.CreateAsync / RetrieveAsync
    Uploads ................ FileUploads.CreateAsync / SendAsync /
                             CompleteAsync / ListAsync / RetrieveAsync

    Authoring .............. NotionText.Run / Split / Plain / PlainBase
                             NotionBlocks.Paragraph / Heading2 / Heading3 /
                                 Callout / Quote / Bullet / Numbered / Toggle /
                                 Divider / Table / TableRow / ImageExternal /
                                 ImageUpload
                             client.CreateChildPageAsync /
                                 AppendChildrenBatchedAsync / UploadFileAsync /
                                 ArchivePageAsync / RetrieveAllChildrenAsync

    Hard limits ............ 2,000 characters per rich-text run
                             100 children per append request
                             100 items per page of results

    Errors ................. NotionApiException (StatusCode,
                                 NotionAPIErrorCode)
                             NotionApiRateLimitException (RetryAfter) on 429
    Retry .................. ClientOptions.RetryPolicy =
                                 new DefaultRetryPolicy()   // opt-in

    Unknown values ......... never throw: UnsupportedBlock,
                             UnknownPropertyValue, UnknownPropertyItem,
                             UnknownProperty, UnknownRichText,
                             UnknownFileObject, UnknownFileObjectWithName,
                             UnknownFileImportResult,
                             UnknownDataSourcePropertyConfig, UnknownObject
