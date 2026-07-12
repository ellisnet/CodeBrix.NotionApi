using System;
using System.Threading;
using System.Threading.Tasks;
using static CodeBrix.NotionApi.ApiEndpoints;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public sealed partial class DatabasesClient : IDatabasesClient
{
    private readonly IRestClient _client;

    public DatabasesClient(IRestClient client)
    {
        _client = client;
    }

    public async Task<Database> RetrieveAsync(string databaseId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(databaseId))
        {
            throw new ArgumentNullException(nameof(databaseId));
        }

        return await _client.GetAsync<Database>(DatabasesApiUrls.Retrieve(databaseId), cancellationToken: cancellationToken);
    }
}
