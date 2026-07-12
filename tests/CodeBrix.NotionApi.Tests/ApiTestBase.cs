using System;
using System.Net.Http;
using CodeBrix.NotionApi;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class ApiTestBase : IDisposable
{
    protected readonly ClientOptions ClientOptions;
    protected readonly FakeServer Server;

    protected ApiTestBase()
    {
        Server = FakeServer.Start();

        ClientOptions = new ClientOptions
        {
            BaseUrl = Server.Urls[0],
            AuthToken = "<Token>",
            HttpClient = new HttpClient(Server.CreateHandler()),
        };
    }

    public void Dispose()
    {
        Server.Stop();
        Server.Dispose();
    }

    protected RequestBuilder CreateGetRequestBuilder(string path)
    {
        return Request.Create()
            .WithPath(path)
            .UsingGet()
            .WithHeader("Authorization", $"Bearer {ClientOptions.AuthToken}")
            .WithHeader("Notion-Version", Constants.DefaultNotionVersion);
    }

    protected RequestBuilder CreatePostRequestBuilder(string path)
    {
        return Request.Create()
            .WithPath(path)
            .UsingPost()
            .WithHeader("Authorization", $"Bearer {ClientOptions.AuthToken}")
            .WithHeader("Notion-Version", Constants.DefaultNotionVersion);
    }

    protected RequestBuilder CreatePatchRequestBuilder(string path)
    {
        return Request.Create()
            .WithPath(path)
            .UsingPatch()
            .WithHeader("Authorization", $"Bearer {ClientOptions.AuthToken}")
            .WithHeader("Notion-Version", Constants.DefaultNotionVersion);
    }

    protected RequestBuilder CreateDeleteRequestBuilder(string path)
    {
        return Request.Create()
            .WithPath(path)
            .UsingDelete()
            .WithHeader("Authorization", $"Bearer {ClientOptions.AuthToken}")
            .WithHeader("Notion-Version", Constants.DefaultNotionVersion);
    }
}
