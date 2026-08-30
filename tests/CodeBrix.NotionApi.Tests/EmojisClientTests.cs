using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

public class EmojisClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly IEmojisClient _emojisClient;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public EmojisClientTests()
    {
        _emojisClient = new EmojisClient(_restClient);
    }

    [Fact]
    public async Task ListAsync_gets_the_custom_emojis_endpoint()
    {
        // Arrange
        var expected = new ListEmojisResponse { Results = new List<CustomEmoji>() };
        _restClient.SetResponse(expected);

        // Act
        var result = await _emojisClient.ListAsync(new ListEmojisRequest(), _cancellationToken);

        // Assert
        Assert.Same(expected, result);
        Assert.Equal("GET", _restClient.LastCall.Method);
        Assert.Equal("/v1/custom_emojis", _restClient.LastCall.Uri);
        Assert.Equal(ApiEndpoints.EmojisApiUrls.List, _restClient.LastCall.Uri);
    }

    [Fact]
    public async Task ListAsync_passes_the_pagination_parameters()
    {
        // Arrange
        _restClient.SetResponse(new ListEmojisResponse { Results = new List<CustomEmoji>() });

        var request = new ListEmojisRequest
        {
            StartCursor = "cursor-1",
            PageSize = 50
        };

        // Act
        await _emojisClient.ListAsync(request, _cancellationToken);

        // Assert
        var queryParams = Assert.IsType<Dictionary<string, string>>(_restClient.LastCall.QueryParams);
        Assert.Equal("cursor-1", queryParams["start_cursor"]);
        Assert.Equal("50", queryParams["page_size"]);
    }
}
