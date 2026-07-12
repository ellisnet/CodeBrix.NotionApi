# CodeBrix.NotionApi

A fully managed .NET client library for the Notion API — a CodeBrix vendored replacement for the Notion.Net package (ported from notion-sdk-net 5.0.0), rebuilt on System.Text.Json with no Newtonsoft.Json or JsonSubTypes dependencies.
CodeBrix.NotionApi has no dependencies other than .NET and a small set of Microsoft-maintained abstractions packages, and is provided as a .NET 10 library and associated `CodeBrix.NotionApi.MitLicenseForever` NuGet package. That single package delivers two assemblies: `CodeBrix.NotionApi` (the Notion API client) and `CodeBrix.JsonPolymorphism` (discriminator-based polymorphic JSON deserialization with fallback types, built on System.Text.Json).

CodeBrix.NotionApi supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.NotionApi supports:

* All of the Notion API endpoint groups covered by Notion.Net 5.0.0 — blocks, pages, databases, data sources, users, comments, search, file uploads, and OAuth authentication
* Discriminator-driven polymorphic models (blocks, rich text, property values, parents, files, and more) deserialized via `CodeBrix.JsonPolymorphism` — unknown variants fall back to the registered fallback type instead of throwing
* Configurable retry policy with Notion rate-limit handling, request logging, and `Microsoft.Extensions.DependencyInjection` registration via `AddNotionClient(...)`
* All JSON operations via `System.Text.Json` — no Newtonsoft.Json, no JsonSubTypes

## Sample Code

### Retrieve the bot user for an integration token

```csharp
using CodeBrix.NotionApi;

var client = NotionClientFactory.Create(new ClientOptions
{
    AuthToken = "ntn_your_integration_token",
});

var me = await client.Users.MeAsync();
Console.WriteLine($"Connected as: {me.Name}");
```

### Query a database

```csharp
using CodeBrix.NotionApi;

var pages = await client.Databases.QueryAsync("database-id", new DatabasesQueryParameters());

foreach (var page in pages.Results)
{
    Console.WriteLine(page.Id);
}
```

## License

The project is licensed under the MIT License. see: https://en.wikipedia.org/wiki/MIT_License
