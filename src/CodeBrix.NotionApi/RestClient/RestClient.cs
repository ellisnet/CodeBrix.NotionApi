using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RestClient : IRestClient
{
    private readonly ClientOptions _options;

    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new RuntimeTypeConverterFactory() },
    };

    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;
    private readonly Uri _baseUri;
    private readonly bool _ownsHttpClient;
    private readonly IRetryPolicy _retryPolicy;
    private bool _disposed;

    /// <summary>
    /// Creates a <see cref="RestClient"/> that uses the <see cref="HttpClient"/> supplied on
    /// <paramref name="options"/> when <see cref="ClientOptions.HttpClient"/> is set, or otherwise
    /// builds and owns a default one.
    /// </summary>
    public RestClient(ClientOptions options)
        : this(options, null)
    {
    }

    /// <summary>
    /// Creates a <see cref="RestClient"/> that obtains its <see cref="HttpClient"/> from
    /// <paramref name="httpClientFactory"/> — resolved fresh per request so
    /// <see cref="IHttpClientFactory"/> handler rotation is preserved.
    /// <para>
    /// Resolution precedence: an explicit <see cref="ClientOptions.HttpClient"/> wins; otherwise
    /// <paramref name="httpClientFactory"/> is used when non-null; otherwise the client builds and owns
    /// a default <see cref="HttpClient"/>. Only the last case is disposed by this instance.
    /// </para>
    /// </summary>
    public RestClient(ClientOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = MergeOptions(options);
        _retryPolicy = options.RetryPolicy;
        _baseUri = new Uri(_options.BaseUrl);
        _httpClientName = Constants.HttpClientName;

        if (options.HttpClient != null)
        {
            // Strategy 1 — caller-supplied client. Used as-is; the caller owns its lifetime.
            if (options.HttpClient.BaseAddress == null)
            {
                options.HttpClient.BaseAddress = _baseUri;
            }

            _httpClient = options.HttpClient;
            _ownsHttpClient = false;
        }
        else if (httpClientFactory != null)
        {
            // Strategy 2 — resolve a fresh client from the factory on every request, so we pick up
            // IHttpClientFactory's periodic handler rotation (stale-DNS / socket-exhaustion safety).
            // Nothing is cached here, so there is nothing for this instance to dispose.
            _httpClientFactory = httpClientFactory;
            _ownsHttpClient = false;
        }
        else
        {
            // Strategy 3 — no client and no factory supplied: build and own a default client.
            _httpClient = CreateDefaultHttpClient(_baseUri);
            _ownsHttpClient = true;
        }
    }

    /// <summary>
    /// Returns the <see cref="HttpClient"/> to use for a single request. When an
    /// <see cref="IHttpClientFactory"/> was supplied, a fresh client is requested on every call (with
    /// its <c>BaseAddress</c> set when the named client did not configure one); otherwise the cached
    /// caller-supplied or internally-owned client is returned.
    /// </summary>
    private HttpClient GetHttpClient()
    {
        if (_httpClientFactory == null)
        {
            return _httpClient;
        }

        var client = _httpClientFactory.CreateClient(_httpClientName);

        if (client.BaseAddress == null)
        {
            client.BaseAddress = _baseUri;
        }

        return client;
    }

    /// <summary>
    /// Builds the default request pipeline used when neither a client nor a factory is supplied:
    /// <c>LoggingHandler → HttpClientHandler</c>. The returned client is owned and disposed by this
    /// <see cref="RestClient"/>.
    /// </summary>
    private static HttpClient CreateDefaultHttpClient(Uri baseUri)
    {
        var pipeline = new LoggingHandler { InnerHandler = new HttpClientHandler() };

        return new HttpClient(pipeline) { BaseAddress = baseUri };
    }

    public async Task<T> GetAsync<T>(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(uri, HttpMethod.Get, queryParams, headers,
            cancellationToken: cancellationToken);

        return await response.ParseStreamAsync<T>(serializerOptions);
    }

    public async Task<T> PostAsync<T>(
        string uri,
        object body,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default)
    {
        void AttachContent(HttpRequestMessage httpRequest)
        {
            if (body == null)
            {
                return;
            }

            var jsonObjectString = JsonSerializer.Serialize(body, DefaultSerializerOptions);
            httpRequest.Content = new StringContent(jsonObjectString, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(
            uri,
            HttpMethod.Post,
            queryParams,
            headers,
            AttachContent,
            basicAuthenticationParameters,
            cancellationToken
        );

        return await response.ParseStreamAsync<T>(serializerOptions);
    }

    public async Task<T> PostAsync<T>(
        string uri,
        ISendFileUploadFormDataParameters formData,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default)
    {
        void AttachContent(HttpRequestMessage httpRequest)
        {
            var fileContent = new StreamContent(formData.File.Data);

            // Parse rather than construct: Notion hands back content types that carry parameters
            // (for example "text/plain; charset=utf-8"), and the MediaTypeHeaderValue constructor
            // rejects anything but a bare media type with a FormatException.
            if (!string.IsNullOrWhiteSpace(formData.File.ContentType))
            {
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(formData.File.ContentType);
            }

            var form = new MultipartFormDataContent
            {
                { fileContent, "file", formData.File.FileName }
            };

            if (!string.IsNullOrEmpty(formData.PartNumber))
            {
                form.Add(new StringContent(formData.PartNumber), "part_number");
            }

            httpRequest.Content = form;
        }

        var response = await SendAsync(
            uri,
            HttpMethod.Post,
            queryParams,
            headers,
            AttachContent,
            basicAuthenticationParameters,
            cancellationToken
        );

        return await response.ParseStreamAsync<T>(serializerOptions);
    }

    public async Task<T> PatchAsync<T>(
        string uri,
        object body,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        void AttachContent(HttpRequestMessage httpRequest)
        {
            var serializedBody = JsonSerializer.Serialize(body, DefaultSerializerOptions);
            httpRequest.Content = new StringContent(serializedBody, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, new HttpMethod("PATCH"), queryParams, headers, AttachContent,
            basicAuthenticationParameters: null, cancellationToken);

        return await response.ParseStreamAsync<T>(serializerOptions);
    }

    public async Task DeleteAsync(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        CancellationToken cancellationToken = default)
    {
        await SendAsync(uri, HttpMethod.Delete, queryParams, headers, null,
            basicAuthenticationParameters: null, cancellationToken);
    }

    public async Task<T> DeleteAsync<T>(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(uri, HttpMethod.Delete, queryParams, headers, null,
            basicAuthenticationParameters: null, cancellationToken);

        return await response.ParseStreamAsync<T>(serializerOptions);
    }

    private static ClientOptions MergeOptions(ClientOptions options)
    {
        return new ClientOptions
        {
            AuthToken = options.AuthToken,
            BaseUrl = options.BaseUrl ?? Constants.BaseUrl,
            NotionVersion = options.NotionVersion ?? Constants.DefaultNotionVersion
        };
    }

    private static async Task<Exception> BuildException(HttpResponseMessage response)
    {
        var errorBody = await response.Content.ReadAsStringAsync();

        NotionApiErrorResponse errorResponse = null;

        if (!string.IsNullOrWhiteSpace(errorBody))
        {
            try
            {
                errorResponse = JsonSerializer.Deserialize<NotionApiErrorResponse>(errorBody, DefaultSerializerOptions);

                if (errorResponse.ErrorCode == NotionAPIErrorCode.RateLimited)
                {
                    var retryAfter = response.Headers.RetryAfter.Delta;

                    return new NotionApiRateLimitException(
                        response.StatusCode,
                        errorResponse.ErrorCode,
                        errorResponse.Message,
                        retryAfter
                    );
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when parsing the notion api response.");
            }
        }

        return new NotionApiException(response.StatusCode, errorResponse?.ErrorCode, errorResponse?.Message);
    }

    private async Task<HttpResponseMessage> SendAsync(
        string requestUri,
        HttpMethod httpMethod,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        Action<HttpRequestMessage> attachContent = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default)
    {
        requestUri = AddQueryString(requestUri, queryParams);

        // Resolved once per logical request. With an IHttpClientFactory this asks the factory for a
        // fresh client (picking up handler rotation); otherwise it returns the cached client.
        var httpClient = GetHttpClient();

        for (var attempt = 0; ; attempt++)
        {
            // HttpRequestMessage is single-use; rebuild it for every attempt.
            using var httpRequest = new HttpRequestMessage(httpMethod, requestUri);

            httpRequest.Headers.Authorization = CreateAuthenticationHeader(basicAuthenticationParameters);
            httpRequest.Headers.Add("Notion-Version", _options.NotionVersion);

            if (headers != null)
            {
                AddHeaders(httpRequest, headers);
            }

            attachContent?.Invoke(httpRequest);

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            if (_retryPolicy == null || !_retryPolicy.ShouldRetry(response, httpMethod, attempt))
            {
                throw await BuildException(response);
            }

            var delay = _retryPolicy.GetDelay(response, attempt);

            Log.Trace(
                "Retry attempt {attempt} after {delay}ms (HTTP {status})",
                attempt + 1,
                (int)delay.TotalMilliseconds,
                (int)response.StatusCode);

            response.Dispose();

            await Task.Delay(delay, cancellationToken);
        }
    }

    private AuthenticationHeaderValue CreateAuthenticationHeader(IBasicAuthenticationParameters basicAuth)
    {
        return basicAuth != null
            ? new AuthenticationHeaderValue("Basic", HeaderHelpers.GetBasicAuthHeaderValue(basicAuth))
            : new AuthenticationHeaderValue("Bearer", _options.AuthToken);
    }

    private static void AddHeaders(HttpRequestMessage request, IDictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }

    private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryParams)
    {
        return queryParams == null ? uri : QueryHelpers.AddQueryString(uri, queryParams);
    }

    /// <summary>
    /// Disposes the underlying <see cref="HttpClient"/>, but only when this
    /// <see cref="RestClient"/> created it internally. A client supplied through
    /// <see cref="ClientOptions.HttpClient"/> — including one from
    /// <see cref="System.Net.Http.IHttpClientFactory"/> — is owned by the caller and is
    /// left untouched.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by this <see cref="RestClient"/>.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> when called from <see cref="Dispose()"/>; <c>false</c> from a finalizer.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _ownsHttpClient)
        {
            _httpClient.Dispose();
        }

        _disposed = true;
    }
}
