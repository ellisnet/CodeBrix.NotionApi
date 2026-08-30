using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface INotionClient : IDisposable
{
    IAuthenticationClient AuthenticationClient { get; }

    IUsersClient Users { get; }

    IDatabasesClient Databases { get; }

    IPagesClient Pages { get; }

    ISearchClient Search { get; }

    IBlocksClient Blocks { get; }

    ICommentsClient Comments { get; }

    IFileUploadsClient FileUploads { get; }

    IDataSourcesClient DataSources { get; }

    IViewsClient Views { get; }

    IEmojisClient Emojis { get; }

    IRestClient RestClient { get; }
}

public class NotionClient : INotionClient
{
    public NotionClient(
        IRestClient restClient,
        IUsersClient users,
        IDatabasesClient databases,
        IPagesClient pages,
        ISearchClient search,
        ICommentsClient comments,
        IBlocksClient blocks,
        IAuthenticationClient authenticationClient,
        IFileUploadsClient fileUploadsClient,
        IDataSourcesClient dataSourcesClient,
        IViewsClient viewsClient,
        IEmojisClient emojisClient)
    {
        RestClient = restClient;
        Users = users;
        Databases = databases;
        Pages = pages;
        Search = search;
        Comments = comments;
        Blocks = blocks;
        AuthenticationClient = authenticationClient;
        FileUploads = fileUploadsClient;
        DataSources = dataSourcesClient;
        Views = viewsClient;
        Emojis = emojisClient;
    }

    public IAuthenticationClient AuthenticationClient { get; }

    public IUsersClient Users { get; }

    public IDatabasesClient Databases { get; }

    public IPagesClient Pages { get; }

    public ISearchClient Search { get; }

    public IBlocksClient Blocks { get; }

    public ICommentsClient Comments { get; }

    public IFileUploadsClient FileUploads { get; }

    public IDataSourcesClient DataSources { get; }

    public IViewsClient Views { get; }

    public IEmojisClient Emojis { get; }

    public IRestClient RestClient { get; }

    private bool _disposed;

    /// <summary>
    /// Disposes this client's underlying <see cref="IRestClient"/>, releasing the
    /// HttpClient it owns. When the HttpClient was supplied by the caller or managed by
    /// IHttpClientFactory (the AddNotionClient DI path), it is left untouched.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by this <see cref="NotionClient"/>.
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

        if (disposing)
        {
            RestClient.Dispose();
        }

        _disposed = true;
    }
}
