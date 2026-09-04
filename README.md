# CodeBrix.NotionApi

A fully managed .NET client library for the Notion API: every endpoint group, the complete Notion object model, and a set of high-level authoring helpers on top. The client targets Notion API version `2026-03-11`.
CodeBrix.NotionApi is provided as a .NET 10 library and associated `CodeBrix.NotionApi.MitLicenseForever` NuGet package, which delivers the single `CodeBrix.NotionApi` assembly.

CodeBrix.NotionApi supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.NotionApi.MitLicenseForever
```

Note that the NuGet package ID and the namespace are different - there is no package named plain `CodeBrix.NotionApi`:

* NuGet package ID: `CodeBrix.NotionApi.MitLicenseForever`
* Assembly and primary namespace: `CodeBrix.NotionApi` - i.e. `using CodeBrix.NotionApi;`

The package pulls in the following automatically; no version pinning is needed in the consuming project:

* `CodeBrix.Json.Extensions.MitLicenseForever` - discriminator-driven polymorphic JSON deserialization with fallback types, built on `System.Text.Json`
* `Microsoft.Extensions.Logging.Abstractions`, `Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.Http` - the logging, dependency-injection and `HttpClient`-factory abstractions used by the client and its DI registration

There are no native libraries and no operating-system restrictions. The only runtime requirements are outbound HTTPS to `https://api.notion.com` and a Notion integration token.

## CodeBrix.NotionApi supports:

* Every Notion API endpoint group — blocks, pages, databases, data sources, views, users, comments, custom emojis, search, file uploads, and OAuth authentication
* Discriminator-driven polymorphic models (blocks, rich text, property values, parents, files, and more) deserialized via the `CodeBrix.Json.Extensions.Polymorphism` types from the `CodeBrix.Json.Extensions.MitLicenseForever` package — unknown variants fall back to the registered fallback type instead of throwing
* Configurable retry policy with Notion rate-limit handling, request logging, and `Microsoft.Extensions.DependencyInjection` registration via `AddNotionClient(...)`
* All JSON handled by `System.Text.Json`
* High-level authoring helpers (`NotionText`, `NotionBlocks`, `NotionAuthoringExtensions`) that handle Notion's 2,000-character rich-text runs and 100-block append limits for you

## Sample Code

### Retrieve the bot user for an integration token

```csharp
using CodeBrix.NotionApi;

// A factory-created INotionClient is IDisposable - dispose it when you are done.
using var client = NotionClientFactory.Instance.Create(new ClientOptions
{
    AuthToken = "ntn_your_integration_token",
});

var me = await client.Users.MeAsync();
Console.WriteLine($"Connected as: {me.Name}");
```

### Query a database

```csharp
using System.Linq;
using CodeBrix.NotionApi;

// A database holds one or more data sources; rows are queried per data source.
var database = await client.Databases.RetrieveAsync("database-id");
var dataSourceId = database.DataSources.First().DataSourceId;

var results = await client.DataSources.QueryAsync(new QueryDataSourceRequest
{
    DataSourceId = dataSourceId,
});

// Results are IQueryDataSourceResponseObject - a Page or a DataSource - so
// filter to the type you want.
foreach (var page in results.Results.OfType<Page>())
{
    Console.WriteLine(page.Id);
}
```

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete API reference and usage guide written for AI coding agents - point your agent at that file when it is writing code against this library.

The polymorphic-JSON attributes and converters this library's models are built on are not declared here; they come from the `CodeBrix.Json.Extensions.MitLicenseForever` package. Read that package's own `AGENT-README.txt` if you are declaring your own polymorphic hierarchies.

Additional sample code and usage examples are available in the `CodeBrix.NotionApi.Tests` project:
https://github.com/ellisnet/CodeBrix.NotionApi/tree/main/tests/CodeBrix.NotionApi.Tests

## License

CodeBrix.NotionApi is licensed under the MIT License - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.NotionApi/blob/main/LICENSE) file.

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.NotionApi/blob/main/THIRD-PARTY-NOTICES.txt).
