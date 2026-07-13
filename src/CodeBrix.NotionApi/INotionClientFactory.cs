namespace CodeBrix.NotionApi;

/// <summary>
/// Creates <see cref="INotionClient"/> instances. Implementations may be registered for dependency
/// injection (see <c>ServiceCollectionExtensions.AddNotionClientFactory</c>) so a single, DI-configured
/// <see cref="System.Net.Http.IHttpClientFactory"/> provides the <see cref="System.Net.Http.HttpClient"/>
/// used by every <see cref="INotionClient"/> the factory creates.
/// </summary>
public interface INotionClientFactory
{
    /// <summary>
    /// Creates a new <see cref="INotionClient"/> configured from <paramref name="options"/>.
    /// <para>
    /// When the factory was given an <see cref="System.Net.Http.IHttpClientFactory"/> and
    /// <paramref name="options"/> does not carry an explicit <see cref="ClientOptions.HttpClient"/>,
    /// the returned client resolves its <see cref="System.Net.Http.HttpClient"/> from that factory per
    /// request. Dispose the returned client when you are done with it (e.g. with a <c>using</c>
    /// statement); doing so only releases resources the client itself owns.
    /// </para>
    /// </summary>
    /// <param name="options">Authentication token and other per-client configuration.</param>
    /// <returns>A new, ready-to-use <see cref="INotionClient"/>.</returns>
    INotionClient Create(ClientOptions options);
}
