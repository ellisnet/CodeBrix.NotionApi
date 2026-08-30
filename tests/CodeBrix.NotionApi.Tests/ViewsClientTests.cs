using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

public class ViewsClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly IViewsClient _viewsClient;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public ViewsClientTests()
    {
        _viewsClient = new ViewsClient(_restClient);
    }

    [Fact]
    public async Task ListAsync_throws_when_request_is_null()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _viewsClient.ListAsync(null, _cancellationToken));
    }

    [Fact]
    public async Task ListAsync_sends_the_supplied_query_parameters()
    {
        // Arrange
        var request = new ListViewsRequest
        {
            DatabaseId = "db-1",
            DataSourceId = "ds-1",
            StartCursor = "cursor-1",
            PageSize = 25
        };

        _restClient.SetResponse(new PaginatedList<View> { Results = new List<View>() });

        // Act
        await _viewsClient.ListAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("GET", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.List(), _restClient.LastCall.Uri);

        var queryParams = Assert.IsType<Dictionary<string, string>>(_restClient.LastCall.QueryParams);
        Assert.Equal("db-1", queryParams["database_id"]);
        Assert.Equal("ds-1", queryParams["data_source_id"]);
        Assert.Equal("cursor-1", queryParams["start_cursor"]);
        Assert.Equal("25", queryParams["page_size"]);
    }

    [Fact]
    public async Task ListAsync_omits_query_parameters_that_were_not_supplied()
    {
        // Arrange
        var request = new ListViewsRequest { DataSourceId = "ds-1" };
        _restClient.SetResponse(new PaginatedList<View> { Results = new List<View>() });

        // Act
        await _viewsClient.ListAsync(request, _cancellationToken);

        // Assert
        var queryParams = Assert.IsType<Dictionary<string, string>>(_restClient.LastCall.QueryParams);
        Assert.Equal(new[] { "data_source_id" }, queryParams.Keys);
    }

    [Fact]
    public async Task CreateAsync_posts_the_request_to_the_views_endpoint()
    {
        // Arrange
        var request = new CreateViewRequest
        {
            DataSourceId = "ds-1",
            Name = "Board",
            Type = ViewType.Board
        };

        _restClient.SetResponse(new View { Id = "view-1" });

        // Act
        var result = await _viewsClient.CreateAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("view-1", result.Id);
        Assert.Equal("POST", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.Create(), _restClient.LastCall.Uri);
        Assert.Same(request, _restClient.LastCall.Body);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task RetrieveAsync_throws_when_the_view_id_is_invalid(string viewId)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _viewsClient.RetrieveAsync(viewId, _cancellationToken));
    }

    [Fact]
    public async Task RetrieveAsync_gets_the_view_by_id()
    {
        // Arrange
        _restClient.SetResponse(new View { Id = "view-1" });

        // Act
        var result = await _viewsClient.RetrieveAsync("view-1", _cancellationToken);

        // Assert
        Assert.Equal("view-1", result.Id);
        Assert.Equal("GET", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.Retrieve("view-1"), _restClient.LastCall.Uri);
    }

    [Fact]
    public async Task UpdateAsync_throws_when_the_view_id_is_missing()
    {
        // Arrange
        var request = new UpdateViewRequest { Name = "Renamed" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _viewsClient.UpdateAsync(request, _cancellationToken));
    }

    [Fact]
    public async Task UpdateAsync_patches_the_view_by_id()
    {
        // Arrange
        var request = new UpdateViewRequest { ViewId = "view-1", Name = "Renamed" };
        _restClient.SetResponse(new View { Id = "view-1" });

        // Act
        await _viewsClient.UpdateAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("PATCH", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.Update("view-1"), _restClient.LastCall.Uri);
        Assert.Same(request, _restClient.LastCall.Body);
    }

    [Fact]
    public async Task DeleteAsync_deletes_the_view_and_returns_it()
    {
        // Arrange
        _restClient.SetResponse(new View { Id = "view-1" });

        // Act
        var result = await _viewsClient.DeleteAsync("view-1", _cancellationToken);

        // Assert
        Assert.Equal("view-1", result.Id);
        Assert.Equal("DELETE", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.Delete("view-1"), _restClient.LastCall.Uri);
    }

    [Fact]
    public async Task CreateQueryAsync_posts_to_the_queries_endpoint()
    {
        // Arrange
        var request = new CreateViewQueryRequest { ViewId = "view-1", PageSize = 10 };
        _restClient.SetResponse(new ViewQueryResponse { Id = "query-1" });

        // Act
        var result = await _viewsClient.CreateQueryAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("query-1", result.Id);
        Assert.Equal("POST", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.CreateQuery("view-1"), _restClient.LastCall.Uri);
    }

    [Fact]
    public async Task CreateQueryAsync_throws_when_the_view_id_is_missing()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _viewsClient.CreateQueryAsync(new CreateViewQueryRequest(), _cancellationToken));
    }

    [Fact]
    public async Task GetQueryResultsAsync_gets_the_query_results()
    {
        // Arrange
        var request = new GetViewQueryResultsRequest
        {
            ViewId = "view-1",
            QueryId = "query-1",
            StartCursor = "cursor-1",
            PageSize = 5
        };

        _restClient.SetResponse(new PaginatedList<PageReference> { Results = new List<PageReference>() });

        // Act
        await _viewsClient.GetQueryResultsAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("GET", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.GetQueryResults("view-1", "query-1"), _restClient.LastCall.Uri);

        var queryParams = Assert.IsType<Dictionary<string, string>>(_restClient.LastCall.QueryParams);
        Assert.Equal("cursor-1", queryParams["start_cursor"]);
        Assert.Equal("5", queryParams["page_size"]);
    }

    [Fact]
    public async Task GetQueryResultsAsync_throws_when_the_query_id_is_missing()
    {
        // Arrange
        var request = new GetViewQueryResultsRequest { ViewId = "view-1" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _viewsClient.GetQueryResultsAsync(request, _cancellationToken));
    }

    [Fact]
    public async Task DeleteQueryAsync_deletes_the_query()
    {
        // Arrange
        var request = new DeleteViewQueryRequest { ViewId = "view-1", QueryId = "query-1" };
        _restClient.SetResponse(new DeletedViewQueryResponse { Id = "query-1", Deleted = true });

        // Act
        var result = await _viewsClient.DeleteQueryAsync(request, _cancellationToken);

        // Assert
        Assert.True(result.Deleted);
        Assert.Equal("DELETE", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.ViewsApiUrls.DeleteQuery("view-1", "query-1"), _restClient.LastCall.Uri);
    }

    [Fact]
    public void View_reports_the_view_object_type()
    {
        // Arrange & Act
        var view = new View();

        // Assert
        Assert.Equal(ObjectType.View, view.Object);
        Assert.Equal("view", ObjectType.View.ToString());
    }
}
