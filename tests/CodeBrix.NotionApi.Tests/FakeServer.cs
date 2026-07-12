using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi.Tests;

// In-repo replacement for the small WireMock.Net surface the upstream notion-sdk-net tests used.
// It deliberately exposes the same fluent names (Given / Request.Create() / Response.Create() /
// scenario states / LogEntries) so the ported test bodies stay as close to upstream as possible.
// No sockets are involved: FakeServer.CreateHandler() returns an HttpMessageHandler that routes
// requests to the registered mappings in-process.
public sealed class FakeServer : IDisposable
{
    private readonly object _gate = new();
    private readonly List<Mapping> _mappings = new();
    private readonly Dictionary<string, string> _scenarioStates = new();

    public List<LogEntry> LogEntries { get; } = new();

    public IReadOnlyList<string> Urls { get; } = new[] { "http://fake-notion.localhost/" };

    public static FakeServer Start() => new();

    public Mapping Given(RequestBuilder requestBuilder)
    {
        var mapping = new Mapping(requestBuilder);

        lock (_gate)
        {
            _mappings.Add(mapping);
        }

        return mapping;
    }

    public HttpMessageHandler CreateHandler() => new RoutingHandler(this);

    public void Stop()
    {
    }

    public void Dispose()
    {
    }

    private async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
    {
        var requestBody = request.Content == null ? null : await request.Content.ReadAsStringAsync();

        lock (_gate)
        {
            LogEntries.Add(new LogEntry(request.Method, request.RequestUri, requestBody));

            foreach (var mapping in _mappings)
            {
                if (mapping.Response == null || !mapping.Matches(request))
                {
                    continue;
                }

                if (mapping.Scenario != null)
                {
                    _scenarioStates.TryGetValue(mapping.Scenario, out var currentState);

                    if (!string.Equals(mapping.WhenState, currentState, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (mapping.SetState != null)
                    {
                        _scenarioStates[mapping.Scenario] = mapping.SetState;
                    }
                }

                return mapping.Response.Build();
            }
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(
                "{\"object\":\"error\",\"status\":404,\"code\":\"object_not_found\"," +
                "\"message\":\"FakeServer: no mapping matched " + request.Method + " " +
                request.RequestUri.AbsolutePath + "\"}",
                Encoding.UTF8, "application/json"),
        };
    }

    private sealed class RoutingHandler : HttpMessageHandler
    {
        private readonly FakeServer _server;

        internal RoutingHandler(FakeServer server) => _server = server;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
            => _server.HandleAsync(request);
    }
}

public sealed class LogEntry
{
    internal LogEntry(HttpMethod method, Uri uri, string body)
    {
        Method = method;
        Uri = uri;
        Body = body;
    }

    public HttpMethod Method { get; }

    public Uri Uri { get; }

    public string Body { get; }
}

public static class Request
{
    public static RequestBuilder Create() => new();
}

public sealed class RequestBuilder
{
    internal HttpMethod Method { get; private set; }

    internal string Path { get; private set; }

    internal List<KeyValuePair<string, string>> Headers { get; } = new();

    public RequestBuilder WithPath(string path)
    {
        Path = path.StartsWith("/") ? path : "/" + path;

        return this;
    }

    public RequestBuilder UsingGet()
    {
        Method = HttpMethod.Get;

        return this;
    }

    public RequestBuilder UsingPost()
    {
        Method = HttpMethod.Post;

        return this;
    }

    public RequestBuilder UsingPatch()
    {
        Method = new HttpMethod("PATCH");

        return this;
    }

    public RequestBuilder UsingDelete()
    {
        Method = HttpMethod.Delete;

        return this;
    }

    public RequestBuilder WithHeader(string name, string value)
    {
        Headers.Add(new KeyValuePair<string, string>(name, value));

        return this;
    }
}

public sealed class Mapping
{
    private readonly RequestBuilder _request;

    internal Mapping(RequestBuilder request) => _request = request;

    internal string Scenario { get; private set; }

    internal string WhenState { get; private set; }

    internal string SetState { get; private set; }

    internal ResponseBuilder Response { get; private set; }

    public Mapping InScenario(string scenario)
    {
        Scenario = scenario;

        return this;
    }

    public Mapping WhenStateIs(string state)
    {
        WhenState = state;

        return this;
    }

    public Mapping WillSetStateTo(string state)
    {
        SetState = state;

        return this;
    }

    public Mapping RespondWith(ResponseBuilder response)
    {
        Response = response;

        return this;
    }

    internal bool Matches(HttpRequestMessage request)
    {
        if (_request.Method != null && request.Method != _request.Method)
        {
            return false;
        }

        if (_request.Path != null
            && Uri.UnescapeDataString(request.RequestUri.AbsolutePath) != _request.Path)
        {
            return false;
        }

        foreach (var header in _request.Headers)
        {
            if (!request.Headers.TryGetValues(header.Key, out var values)
                || !values.Contains(header.Value))
            {
                return false;
            }
        }

        return true;
    }
}

public static class Response
{
    public static ResponseBuilder Create() => new();
}

public sealed class ResponseBuilder
{
    private int _statusCode = 200;
    private string _body;
    private readonly List<KeyValuePair<string, string>> _headers = new();

    public ResponseBuilder WithStatusCode(int statusCode)
    {
        _statusCode = statusCode;

        return this;
    }

    public ResponseBuilder WithBody(string body)
    {
        _body = body;

        return this;
    }

    public ResponseBuilder WithHeader(string name, string value)
    {
        _headers.Add(new KeyValuePair<string, string>(name, value));

        return this;
    }

    internal HttpResponseMessage Build()
    {
        var response = new HttpResponseMessage((HttpStatusCode)_statusCode);

        if (_body != null)
        {
            response.Content = new StringContent(_body, Encoding.UTF8, "application/json");
        }

        foreach (var header in _headers)
        {
            if (!response.Headers.TryAddWithoutValidation(header.Key, header.Value))
            {
                response.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return response;
    }
}
