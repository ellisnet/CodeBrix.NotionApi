using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

public class CommentsClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly ICommentsClient _commentsClient;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public CommentsClientTests()
    {
        _commentsClient = new CommentsClient(_restClient);
    }

    [Fact]
    public async Task RetrieveSingleAsync_gets_the_comment_by_id()
    {
        // Arrange
        var expected = new Comment { Id = "comment-1" };
        _restClient.SetResponse(expected);

        // Act
        var result = await _commentsClient.RetrieveSingleAsync(
            new RetrieveSingleCommentRequest { CommentId = "comment-1" }, _cancellationToken);

        // Assert
        Assert.Same(expected, result);
        Assert.Equal("GET", _restClient.LastCall.Method);
        Assert.Equal("/v1/comments/comment-1", _restClient.LastCall.Uri);
    }

    [Fact]
    public async Task UpdateAsync_patches_the_comment_by_id()
    {
        // Arrange
        var request = new UpdateCommentRequest
        {
            CommentId = "comment-1",
            RichText = new List<RichTextBase>
            {
                new RichTextText { Text = new Text { Content = "edited" } }
            }
        };

        _restClient.SetResponse(new Comment { Id = "comment-1" });

        // Act
        await _commentsClient.UpdateAsync(request, _cancellationToken);

        // Assert
        Assert.Equal("PATCH", _restClient.LastCall.Method);
        Assert.Equal("/v1/comments/comment-1", _restClient.LastCall.Uri);
        Assert.Same(request, _restClient.LastCall.Body);
    }

    [Fact]
    public async Task DeleteAsync_deletes_the_comment_by_id()
    {
        // Act
        await _commentsClient.DeleteAsync("comment-1", _cancellationToken);

        // Assert
        Assert.Equal("DELETE", _restClient.LastCall.Method);
        Assert.Equal("/v1/comments/comment-1", _restClient.LastCall.Uri);
    }

    [Fact]
    public void UpdateCommentRequest_keeps_the_comment_id_out_of_the_body()
    {
        // Arrange
        var request = new UpdateCommentRequest
        {
            CommentId = "comment-1",
            RichText = new List<RichTextBase>
            {
                new RichTextText { Text = new Text { Content = "edited" } }
            }
        };

        // Act
        var json = System.Text.Json.JsonSerializer.Serialize(request, RestClient.DefaultSerializerOptions);

        // Assert - CommentId is a path parameter, so it must not appear in the PATCH body
        Assert.DoesNotContain("comment-1", json);
        Assert.Contains("\"rich_text\"", json);
    }
}
