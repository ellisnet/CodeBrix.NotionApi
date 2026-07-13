using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;

namespace CodeBrix.NotionApi.Tests;

// In-repo replacement for the Moq-based IRestClient mocks the upstream notion-sdk-net unit tests
// used: canned responses are registered per response type with SetResponse(), and every call is
// recorded so tests can assert the URL, body, and cancellation token that the API client passed.
public sealed class RecordingRestClient : IRestClient
{
    public sealed class RecordedCall
    {
        public string Method { get; init; }

        public string Uri { get; init; }

        public object Body { get; init; }

        public ISendFileUploadFormDataParameters FormData { get; init; }

        public IEnumerable<KeyValuePair<string, string>> QueryParams { get; init; }

        public IDictionary<string, string> Headers { get; init; }

        public IBasicAuthenticationParameters BasicAuthentication { get; init; }

        public CancellationToken CancellationToken { get; init; }
    }

    private readonly Dictionary<Type, object> _responses = new();

    public List<RecordedCall> Calls { get; } = new();

    public RecordedCall LastCall => Calls.Last();

    public void SetResponse<T>(T response) => _responses[typeof(T)] = response;

    public Task<T> GetAsync<T>(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        Calls.Add(new RecordedCall
        {
            Method = "GET",
            Uri = uri,
            QueryParams = queryParams,
            Headers = headers,
            CancellationToken = cancellationToken,
        });

        return Task.FromResult(GetResponse<T>());
    }

    public Task<T> PostAsync<T>(
        string uri,
        object body,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default)
    {
        Calls.Add(new RecordedCall
        {
            Method = "POST",
            Uri = uri,
            Body = body,
            QueryParams = queryParams,
            Headers = headers,
            BasicAuthentication = basicAuthenticationParameters,
            CancellationToken = cancellationToken,
        });

        return Task.FromResult(GetResponse<T>());
    }

    public Task<T> PostAsync<T>(
        string uri,
        ISendFileUploadFormDataParameters formData,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default)
    {
        Calls.Add(new RecordedCall
        {
            Method = "POST",
            Uri = uri,
            FormData = formData,
            QueryParams = queryParams,
            Headers = headers,
            BasicAuthentication = basicAuthenticationParameters,
            CancellationToken = cancellationToken,
        });

        return Task.FromResult(GetResponse<T>());
    }

    public Task<T> PatchAsync<T>(
        string uri,
        object body,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        Calls.Add(new RecordedCall
        {
            Method = "PATCH",
            Uri = uri,
            Body = body,
            QueryParams = queryParams,
            Headers = headers,
            CancellationToken = cancellationToken,
        });

        return Task.FromResult(GetResponse<T>());
    }

    public Task DeleteAsync(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        CancellationToken cancellationToken = default)
    {
        Calls.Add(new RecordedCall
        {
            Method = "DELETE",
            Uri = uri,
            QueryParams = queryParams,
            Headers = headers,
            CancellationToken = cancellationToken,
        });

        return Task.CompletedTask;
    }

    private T GetResponse<T>()
        => _responses.TryGetValue(typeof(T), out var response) ? (T)response : default;

    // No owned resources; the fake never creates an HttpClient. Present only to satisfy IRestClient : IDisposable.
    public void Dispose()
    {
    }
}
