# CodeBrix.NotionApi

A fully managed .NET client library for the Notion API — a CodeBrix vendored replacement for the Notion.Net package (ported from notion-sdk-net 5.0.0), rebuilt on System.Text.Json with no Newtonsoft.Json or JsonSubTypes dependencies.
CodeBrix.NotionApi is provided as a .NET 10 library and associated `CodeBrix.NotionApi.MitLicenseForever` NuGet package, which delivers the single `CodeBrix.NotionApi` assembly (the Notion API client). Its only non-Microsoft dependency is the `CodeBrix.Json.Extensions.MitLicenseForever` NuGet package, which supplies discriminator-based polymorphic JSON deserialization with fallback types (built on System.Text.Json); the remaining dependencies are a small set of Microsoft-maintained abstractions packages.

CodeBrix.NotionApi supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.NotionApi supports:

* All of the Notion API endpoint groups covered by Notion.Net 5.0.0 — blocks, pages, databases, data sources, users, comments, search, file uploads, and OAuth authentication
* Discriminator-driven polymorphic models (blocks, rich text, property values, parents, files, and more) deserialized via the `CodeBrix.Json.Extensions.Polymorphism` types from the `CodeBrix.Json.Extensions.MitLicenseForever` package — unknown variants fall back to the registered fallback type instead of throwing
* Configurable retry policy with Notion rate-limit handling, request logging, and `Microsoft.Extensions.DependencyInjection` registration via `AddNotionClient(...)`
* All JSON operations via `System.Text.Json` — no Newtonsoft.Json, no JsonSubTypes

## Sample Code

### Retrieve the bot user for an integration token

```csharp
using CodeBrix.NotionApi;

var client = NotionClientFactory.Instance.Create(new ClientOptions
{
    AuthToken = "ntn_your_integration_token",
});

var me = await client.Users.MeAsync();
Console.WriteLine($"Connected as: {me.Name}");
```

### Query a database

```csharp
using CodeBrix.NotionApi;

// A database holds one or more data sources; rows are queried per data source.
var database = await client.Databases.RetrieveAsync("database-id");
var dataSourceId = database.DataSources.First().DataSourceId;

var pages = await client.DataSources.QueryAsync(new QueryDataSourceRequest
{
    DataSourceId = dataSourceId,
});

foreach (var page in pages.Results)
{
    Console.WriteLine(page.Id);
}
```

## License

The project is licensed under the MIT License. see: https://en.wikipedia.org/wiki/MIT_License
