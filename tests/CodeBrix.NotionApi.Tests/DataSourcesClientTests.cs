using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class DataSourcesClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly IDataSourcesClient _dataSourcesClient;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public DataSourcesClientTests()
    {
        _dataSourcesClient = new DataSourcesClient(_restClient);
    }

    #region RetrieveAsync Tests

    [Fact]
    public async Task RetrieveAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        RetrieveDataSourceRequest request = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataSourcesClient.RetrieveAsync(request, _cancellationToken));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task RetrieveAsync_ShouldThrowArgumentException_WhenDataSourceIdIsInvalid(string dataSourceId)
    {
        // Arrange
        var request = new RetrieveDataSourceRequest
        {
            DataSourceId = dataSourceId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _dataSourcesClient.RetrieveAsync(request, _cancellationToken));
        Assert.Equal("DataSourceId cannot be null or empty. (Parameter 'DataSourceId')", exception.Message);
    }

    [Fact]
    public async Task RetrieveAsync_ShouldReturnDataSource()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var expectedDataSource = new DataSource
        {
            Id = dataSourceId,
            Title =
            [
                new RichTextText
                {
                    Text = new Text
                    {
                        Content = "Test Data Source"
                    }
                }
            ]
        };

        var request = new RetrieveDataSourceRequest
        {
            DataSourceId = dataSourceId
        };

        _restClient.SetResponse(expectedDataSource);

        // Act
        var result = await _dataSourcesClient.RetrieveAsync(request, _cancellationToken);

        // Assert
        Assert.Equal(expectedDataSource, result);
        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.DataSourcesApiUrls.Retrieve(request), _restClient.LastCall.Uri);
        Assert.Equal(_cancellationToken, _restClient.LastCall.CancellationToken);
    }

    #endregion RetrieveAsync Tests

    #region ListDataSourceTemplatesAsync Tests

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        ListDataSourceTemplatesRequest request = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task ListDataSourceTemplatesAsync_ShouldThrowArgumentException_WhenDataSourceIdIsInvalid(string dataSourceId)
    {
        // Arrange
        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken));
        Assert.Equal("DataSourceId cannot be null or empty. (Parameter 'DataSourceId')", exception.Message);
    }

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldReturnTemplates_WithValidRequest()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var expectedResponse = new ListDataSourceTemplatesResponse
        {
            Templates =
            [
                new DataSourceTemplate
                {
                    Id = "template-1",
                    Name = "Default Template",
                    IsDefault = true
                },
                new DataSourceTemplate
                {
                    Id = "template-2",
                    Name = "Custom Template",
                    IsDefault = false
                }
            ],
            HasMore = false,
            NextCursor = null
        };

        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId
        };

        var expectedEndpoint = $"/v1/data_sources/{dataSourceId}/templates";
        var expectedQueryParams = new Dictionary<string, string>
        {
            { "name", null },
            { "start_cursor", null },
            { "page_size", null }
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var result = await _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken);

        // Assert
        Assert.Equal(expectedResponse, result);
        Assert.Equal(expectedEndpoint, _restClient.LastCall.Uri);
        Assert.Equal(expectedQueryParams, _restClient.LastCall.QueryParams);
        Assert.Equal(2, result.Templates.Count());
        Assert.False(result.HasMore);
        Assert.Null(result.NextCursor);
    }

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldPassAllQueryParameters_WhenProvided()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var templateName = "Custom Template";
        var startCursor = "cursor123";
        var pageSize = 50;

        var expectedResponse = new ListDataSourceTemplatesResponse
        {
            Templates =
            [
                new DataSourceTemplate
                {
                    Id = "template-1",
                    Name = templateName,
                    IsDefault = false
                }
            ],
            HasMore = true,
            NextCursor = "next-cursor456"
        };

        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId,
            Name = templateName,
            StartCursor = startCursor,
            PageSize = pageSize
        };

        var expectedEndpoint = $"/v1/data_sources/{dataSourceId}/templates";
        var expectedQueryParams = new Dictionary<string, string>
        {
            { "name", templateName },
            { "start_cursor", startCursor },
            { "page_size", pageSize.ToString() }
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var result = await _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken);

        // Assert
        Assert.Equal(expectedResponse, result);
        Assert.Single(result.Templates);
        Assert.True(result.HasMore);
        Assert.Equal("next-cursor456", result.NextCursor);

        // Verify the REST client was called with correct parameters
        Assert.Single(_restClient.Calls);
        Assert.Equal(expectedEndpoint, _restClient.LastCall.Uri);
        Assert.Equal(expectedQueryParams, _restClient.LastCall.QueryParams);
        Assert.Equal(_cancellationToken, _restClient.LastCall.CancellationToken);
    }

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldReturnEmptyTemplates_WhenNoTemplatesFound()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var expectedResponse = new ListDataSourceTemplatesResponse
        {
            Templates = [],
            HasMore = false,
            NextCursor = null
        };

        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId
        };

        var expectedEndpoint = $"/v1/data_sources/{dataSourceId}/templates";

        _restClient.SetResponse(expectedResponse);

        // Act
        var result = await _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken);

        // Assert
        Assert.Equal(expectedResponse, result);
        Assert.Equal(expectedEndpoint, _restClient.LastCall.Uri);
        Assert.Empty(result.Templates);
        Assert.False(result.HasMore);
        Assert.Null(result.NextCursor);
    }

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldHandleNullPageSize_InQueryParameters()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId,
            Name = "Test Template",
            StartCursor = "cursor123",
            PageSize = null // Explicitly null
        };

        var expectedResponse = new ListDataSourceTemplatesResponse
        {
            Templates = [],
            HasMore = false,
            NextCursor = null
        };

        var expectedEndpoint = $"/v1/data_sources/{dataSourceId}/templates";
        var expectedQueryParams = new Dictionary<string, string>
        {
            { "name", "Test Template" },
            { "start_cursor", "cursor123" },
            { "page_size", null } // Should be null when PageSize is null
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var result = await _dataSourcesClient.ListDataSourceTemplatesAsync(request, _cancellationToken);

        // Assert
        Assert.Equal(expectedResponse, result);

        // Verify the REST client was called with correct parameters
        Assert.Single(_restClient.Calls);
        Assert.Equal(expectedEndpoint, _restClient.LastCall.Uri);
        Assert.Equal(expectedQueryParams, _restClient.LastCall.QueryParams);
        Assert.Equal(_cancellationToken, _restClient.LastCall.CancellationToken);
    }

    [Fact]
    public async Task ListDataSourceTemplatesAsync_ShouldUseCancellationToken()
    {
        // Arrange
        var dataSourceId = "test-data-source-id";
        var customCancellationToken = new CancellationTokenSource().Token;
        var request = new ListDataSourceTemplatesRequest
        {
            DataSourceId = dataSourceId
        };

        var expectedResponse = new ListDataSourceTemplatesResponse
        {
            Templates = [],
            HasMore = false,
            NextCursor = null
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var result = await _dataSourcesClient.ListDataSourceTemplatesAsync(request, customCancellationToken);

        // Assert
        Assert.Equal(expectedResponse, result);

        // Verify the custom cancellation token was used
        Assert.Single(_restClient.Calls);
        Assert.Equal(customCancellationToken, _restClient.LastCall.CancellationToken);
    }

    #endregion ListDataSourceTemplatesAsync Tests
}
