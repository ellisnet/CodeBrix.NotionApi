using System;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected readonly INotionClient Client;
    protected readonly string ParentPageId;
    protected readonly string ParentDatabaseId;

    protected IntegrationTestBase()
    {
        // These tests exercise the real Notion API and are opt-in: they run only when the
        // NOTION_AUTH_TOKEN / NOTION_PARENT_PAGE_ID / NOTION_PARENT_DATABASE_ID environment
        // variables are present; otherwise every test in the class is skipped.
        Assert.SkipWhen(
            Environment.GetEnvironmentVariable("NOTION_AUTH_TOKEN") == null,
            "NOTION_AUTH_TOKEN environment variable is not set; skipping Notion integration tests.");

        var options = new ClientOptions
        {
            AuthToken = Environment.GetEnvironmentVariable("NOTION_AUTH_TOKEN"),
            RetryPolicy = new DefaultRetryPolicy()
        };

        Client = NotionClientFactory.Create(options);

        ParentPageId = GetEnvironmentVariableRequired("NOTION_PARENT_PAGE_ID");
        ParentDatabaseId = GetEnvironmentVariableRequired("NOTION_PARENT_DATABASE_ID");
    }

    protected static string GetEnvironmentVariableRequired(string envName)
    {
        Assert.SkipWhen(
            Environment.GetEnvironmentVariable(envName) == null,
            $"{envName} environment variable is not set; skipping Notion integration tests.");

        return Environment.GetEnvironmentVariable(envName);
    }
}
