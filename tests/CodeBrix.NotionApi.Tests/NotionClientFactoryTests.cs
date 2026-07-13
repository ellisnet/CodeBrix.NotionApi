using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SilverAssertions;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class NotionClientFactoryTests
{
    private const string BaseUrl = "https://api.notion.example/";

    private static ClientOptions Options()
        => new ClientOptions { AuthToken = "test-token", BaseUrl = BaseUrl };

    private static ClientOptions Options(HttpClient httpClient)
        => new ClientOptions { AuthToken = "test-token", BaseUrl = BaseUrl, HttpClient = httpClient };

    [Fact]
    public async Task RestClient_resolves_a_client_from_the_factory_on_every_request()
    {
        //Arrange
        var factory = new CountingHttpClientFactory();
        var restClient = new RestClient(Options(), factory);

        //Act — three logical requests
        await restClient.GetAsync<Probe>("v1/one", cancellationToken: TestContext.Current.CancellationToken);
        await restClient.GetAsync<Probe>("v1/two", cancellationToken: TestContext.Current.CancellationToken);
        await restClient.GetAsync<Probe>("v1/three", cancellationToken: TestContext.Current.CancellationToken);

        //Assert — one CreateClient call per request (so handler rotation is possible)
        factory.CreateClientCallCount.Should().Be(3);
    }

    [Fact]
    public async Task Dispose_disposes_the_internally_owned_HttpClient()
    {
        //Arrange — no HttpClient and no factory: the client builds and owns its own HttpClient
        var restClient = new RestClient(Options());
        restClient.Dispose();

        //Act / Assert — the owned HttpClient was disposed, so a subsequent request throws
        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            restClient.GetAsync<Probe>("v1/thing", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Dispose_does_not_dispose_a_caller_supplied_HttpClient()
    {
        //Arrange
        var handler = new StubHandler();
        var suppliedClient = new HttpClient(handler);
        var restClient = new RestClient(Options(suppliedClient));

        //Act
        restClient.Dispose();

        //Assert — the caller's client is untouched: its handler was not disposed and it still works
        handler.Disposed.Should().BeFalse();
        var probe = await suppliedClient.GetAsync(BaseUrl, TestContext.Current.CancellationToken);
        probe.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_uses_the_factory_supplied_via_SetHttpClientFactory()
    {
        //Arrange
        var httpClientFactory = new CountingHttpClientFactory();
        var factory = new NotionClientFactory();
        factory.SetHttpClientFactory(httpClientFactory);

        //Act
        using var client = factory.Create(Options());
        await client.RestClient.GetAsync<Probe>("v1/users/me", cancellationToken: TestContext.Current.CancellationToken);

        //Assert
        httpClientFactory.CreateClientCallCount.Should().Be(1);
    }

    [Fact]
    public void SetHttpClientFactory_throws_when_a_factory_was_already_set()
    {
        var factory = new NotionClientFactory(new CountingHttpClientFactory());

        Assert.Throws<InvalidOperationException>(
            () => factory.SetHttpClientFactory(new CountingHttpClientFactory()));
    }

    [Fact]
    public void SetHttpClientFactory_throws_after_a_client_has_been_created()
    {
        var factory = new NotionClientFactory();
        using var _ = factory.Create(Options());

        Assert.Throws<InvalidOperationException>(
            () => factory.SetHttpClientFactory(new CountingHttpClientFactory()));
    }

    [Fact]
    public void SetHttpClientFactory_throws_on_null() =>
        Assert.Throws<ArgumentNullException>(
            () => new NotionClientFactory().SetHttpClientFactory(null));

    // A throwaway response body that deserializes from "{}".
    private sealed class Probe
    {
    }

    // Returns a canned success response and records whether it was disposed.
    private sealed class StubHandler : HttpMessageHandler
    {
        public bool Disposed { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json"),
            });

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }

    // Counts CreateClient calls; each returns a fresh client over a fresh stub handler.
    private sealed class CountingHttpClientFactory : IHttpClientFactory
    {
        public int CreateClientCallCount { get; private set; }

        public HttpClient CreateClient(string name)
        {
            CreateClientCallCount++;
            return new HttpClient(new StubHandler());
        }
    }
}
