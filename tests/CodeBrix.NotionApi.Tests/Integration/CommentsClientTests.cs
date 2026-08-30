using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;

[Collection(NotionIntegrationCollection.Name)]
public class CommentsClientTests : IntegrationTestBase, IAsyncLifetime
{
    private Page _page;

    public async ValueTask InitializeAsync()
    {
        _page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder.Create(
                new PageParentRequest { PageId = ParentPageId }
            ).Build()
        );
    }

    public async ValueTask DisposeAsync()
    {
        await Client.Pages.UpdateAsync(_page.Id, new PagesUpdateParameters { InTrash = true });
    }

    [Fact]
    public async Task ShouldCreatePageComment()
    {
        // Arrange
        var parameters = CreateCommentRequest.CreatePageComment(
            new ParentPageInput { PageId = _page.Id },
            new List<RichTextBaseInput> { new RichTextTextInput { Text = new Text { Content = "This is a comment" } } }
        );

        // Act
        var response = await Client.Comments.CreateAsync(parameters, cancellationToken: TestContext.Current.CancellationToken);

        // Arrange

        Assert.NotNull(response.Parent);
        Assert.NotNull(response.Id);
        Assert.NotNull(response.DiscussionId);

        Assert.NotNull(response.RichText);
        Assert.Single(response.RichText);
        var richText = Assert.IsType<RichTextText>(response.RichText.First());
        Assert.Equal("This is a comment", richText.Text.Content);

        var pageParent = Assert.IsType<PageParent>(response.Parent);
        Assert.Equal(_page.Id, pageParent.PageId);
    }

    [Fact]
    public async Task ShouldCreateADiscussionComment()
    {
        // Arrange
        var comment = await Client.Comments.CreateAsync(
            CreateCommentRequest.CreatePageComment(
                new ParentPageInput { PageId = _page.Id },
                new List<RichTextBaseInput>
                {
                    new RichTextTextInput { Text = new Text { Content = "This is a comment" } }
                }
            )
        , cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var response = await Client.Comments.CreateAsync(
            CreateCommentRequest.CreateDiscussionComment(
                comment.DiscussionId,
                new List<RichTextBaseInput>
                {
                    new RichTextTextInput { Text = new Text { Content = "This is a sub comment" } }
                }
            )
        , cancellationToken: TestContext.Current.CancellationToken);

        // Arrange
        Assert.NotNull(response.Parent);
        Assert.NotNull(response.Id);
        Assert.Equal(comment.DiscussionId, response.DiscussionId);

        Assert.NotNull(response.RichText);
        Assert.Single(response.RichText);
        var richText = Assert.IsType<RichTextText>(response.RichText.First());
        Assert.Equal("This is a sub comment", richText.Text.Content);

        var pageParent = Assert.IsType<PageParent>(response.Parent);
        Assert.Equal(_page.Id, pageParent.PageId);
    }

    [Fact]
    public async Task ShouldRetrieveSingleComment()
    {
        // Arrange
        var created = await Client.Comments.CreateAsync(
            CreateCommentRequest.CreatePageComment(
                new ParentPageInput { PageId = _page.Id },
                new List<RichTextBaseInput>
                {
                    new RichTextTextInput { Text = new Text { Content = "Comment to retrieve" } }
                }
            )
        , cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var retrieved = await Client.Comments.RetrieveSingleAsync(
            new RetrieveSingleCommentRequest { CommentId = created.Id }
        , cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(created.Id, retrieved.Id);
        Assert.Equal(created.DiscussionId, retrieved.DiscussionId);
        var richText = Assert.IsType<RichTextText>(retrieved.RichText.First());
        Assert.Equal("Comment to retrieve", richText.Text.Content);
    }

    [Fact]
    public async Task ShouldUpdateComment()
    {
        // Arrange
        var created = await Client.Comments.CreateAsync(
            CreateCommentRequest.CreatePageComment(
                new ParentPageInput { PageId = _page.Id },
                new List<RichTextBaseInput>
                {
                    new RichTextTextInput { Text = new Text { Content = "Original text" } }
                }
            )
        , cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var updated = await Client.Comments.UpdateAsync(new UpdateCommentRequest
        {
            CommentId = created.Id,
            RichText = new List<RichTextBase>
            {
                new RichTextText { Text = new Text { Content = "Updated text" } }
            }
        }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated.Id);
        var richText = Assert.IsType<RichTextText>(updated.RichText.First());
        Assert.Equal("Updated text", richText.Text.Content);
    }

    [Fact]
    public async Task ShouldDeleteComment()
    {
        // Arrange
        var created = await Client.Comments.CreateAsync(
            CreateCommentRequest.CreatePageComment(
                new ParentPageInput { PageId = _page.Id },
                new List<RichTextBaseInput>
                {
                    new RichTextTextInput { Text = new Text { Content = "Comment to delete" } }
                }
            )
        , cancellationToken: TestContext.Current.CancellationToken);

        // Act
        await Client.Comments.DeleteAsync(created.Id, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var comments = await Client.Comments.RetrieveAsync(
            new RetrieveCommentsRequest { BlockId = _page.Id }
        , cancellationToken: TestContext.Current.CancellationToken);

        Assert.DoesNotContain(comments.Results, c => c.Id == created.Id);
    }
}
