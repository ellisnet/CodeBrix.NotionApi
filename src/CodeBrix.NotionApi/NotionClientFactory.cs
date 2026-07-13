using System;
using System.Net.Http;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Default <see cref="INotionClientFactory"/>. Create one and register it for dependency injection, or
/// use the shared <see cref="Instance"/> in non-DI code.
/// <para>
/// When an <see cref="IHttpClientFactory"/> is supplied — through the constructor or
/// <see cref="SetHttpClientFactory"/> — every <see cref="INotionClient"/> (and its underlying
/// <c>RestClient</c>) that this factory creates obtains its <see cref="HttpClient"/> from that
/// <see cref="IHttpClientFactory"/>, resolved per request so handler rotation is preserved.
/// </para>
/// </summary>
public class NotionClientFactory : INotionClientFactory
{
    private static NotionClientFactory _instance;
    private static readonly object InstanceLock = new();

    private readonly object _stateLock = new();
    private IHttpClientFactory _httpClientFactory;
    private bool _clientCreated;

    /// <summary>
    /// Creates a factory with no <see cref="IHttpClientFactory"/>. Clients it creates manage their own
    /// <see cref="HttpClient"/> unless one is supplied on <see cref="ClientOptions"/> or an
    /// <see cref="IHttpClientFactory"/> is later set via <see cref="SetHttpClientFactory"/>.
    /// </summary>
    public NotionClientFactory()
    {
    }

    /// <summary>
    /// Creates a factory that supplies every client it creates with an <see cref="HttpClient"/> obtained
    /// (per request) from <paramref name="httpClientFactory"/>. This is the recommended constructor for
    /// dependency-injection registration.
    /// </summary>
    /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> to source clients from.</param>
    public NotionClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// A lazily-created, process-wide singleton for non-DI code. Created once on first access and reused
    /// thereafter. Configure its <see cref="IHttpClientFactory"/> with <see cref="SetHttpClientFactory"/>
    /// before creating any client.
    /// </summary>
    public static NotionClientFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (InstanceLock)
                {
                    _instance ??= new NotionClientFactory();
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// Sets the <see cref="IHttpClientFactory"/> used for all clients this factory subsequently creates.
    /// <para>
    /// This is a set-once operation: it throws if an <see cref="IHttpClientFactory"/> has already been
    /// set (via constructor or a prior call) or if this factory has already created a client. Prefer the
    /// <see cref="NotionClientFactory(IHttpClientFactory)"/> constructor where you control construction;
    /// use this method to configure the shared <see cref="Instance"/>.
    /// </para>
    /// </summary>
    /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> to source clients from.</param>
    /// <exception cref="ArgumentNullException"><paramref name="httpClientFactory"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    /// A factory has already been set, or a client has already been created by this instance.
    /// </exception>
    public void SetHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        if (httpClientFactory == null)
        {
            throw new ArgumentNullException(nameof(httpClientFactory));
        }

        lock (_stateLock)
        {
            if (_httpClientFactory != null)
            {
                throw new InvalidOperationException(
                    "An IHttpClientFactory has already been set on this NotionClientFactory; it can only be set once.");
            }

            if (_clientCreated)
            {
                throw new InvalidOperationException(
                    "An IHttpClientFactory cannot be set after this NotionClientFactory has already created a client.");
            }

            _httpClientFactory = httpClientFactory;
        }
    }

    /// <inheritdoc />
    public INotionClient Create(ClientOptions options)
    {
        IHttpClientFactory httpClientFactory;

        lock (_stateLock)
        {
            _clientCreated = true;
            httpClientFactory = _httpClientFactory;
        }

        var restClient = new RestClient(options, httpClientFactory);

        return new NotionClient(
            restClient
            , new UsersClient(restClient)
            , new DatabasesClient(restClient)
            , new PagesClient(restClient)
            , new SearchClient(restClient)
            , new CommentsClient(restClient)
            , new BlocksClient(restClient)
            , new AuthenticationClient(restClient)
            , new FileUploadsClient(restClient)
            , new DataSourcesClient(restClient)
        );
    }
}
