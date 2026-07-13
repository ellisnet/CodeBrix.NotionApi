using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;
using SilverAssertions;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class NotionAuthoringExtensionsTests : ApiTestBase
{
    private const string EmptyBlockList = "{\"object\":\"list\",\"results\":[],\"has_more\":false}";

    private readonly INotionClient _client;

    public NotionAuthoringExtensionsTests() => _client = NotionClientFactory.Instance.Create(ClientOptions);

    [Fact]
    public void GuessContentType_maps_common_extensions()
    {
        NotionAuthoringExtensions.GuessContentType("a.png").Should().Be("image/png");
        NotionAuthoringExtensions.GuessContentType("a.JPG").Should().Be("image/jpeg");
        NotionAuthoringExtensions.GuessContentType("a.bin").Should().Be("application/octet-stream");
    }

    [Fact]
    public async Task AppendChildrenBatchedAsync_splits_into_requests_of_100()
    {
        //Arrange
        var blockId = "11111111-1111-1111-1111-111111111111";
        var path = ApiEndpoints.BlocksApiUrls.AppendChildren(blockId);
        Server.Given(CreatePatchRequestBuilder(path))
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(EmptyBlockList));

        var blocks = Enumerable.Range(0, 250)
            .Select(_ => (IBlockObjectRequest)NotionBlocks.Paragraph("x"))
            .ToList();

        //Act
        await _client.AppendChildrenBatchedAsync(blockId, blocks, throttleMs: 0, cancellationToken: TestContext.Current.CancellationToken);

        //Assert — 250 blocks => 100 + 100 + 50 = 3 requests
        Server.LogEntries.Count(e => e.Method.Method == "PATCH" && e.Uri.AbsolutePath == path).Should().Be(3);
    }

    [Fact]
    public async Task CreateChildPageAsync_posts_a_title_property()
    {
        //Arrange
        var path = ApiEndpoints.PagesApiUrls.Create();
        Server.Given(CreatePostRequestBuilder(path))
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(PageJson("p1")));

        //Act
        var page = await _client.CreateChildPageAsync("parent-id", "My Title", TestContext.Current.CancellationToken);

        //Assert
        page.Id.Should().Be("p1");
        var body = Server.LogEntries.Single(e => e.Method.Method == "POST" && e.Uri.AbsolutePath == path).Body;
        body.Should().Contain("\"title\"");
        body.Should().Contain("My Title");
    }

    [Fact]
    public async Task ArchivePageAsync_sets_in_trash_true()
    {
        //Arrange
        var path = ApiEndpoints.PagesApiUrls.Update("p1");
        Server.Given(CreatePatchRequestBuilder(path))
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(PageJson("p1")));

        //Act
        await _client.ArchivePageAsync("p1", TestContext.Current.CancellationToken);

        //Assert
        var body = Server.LogEntries.Single(e => e.Method.Method == "PATCH" && e.Uri.AbsolutePath == path).Body;
        body.Should().Contain("\"in_trash\":true");
    }

    [Fact]
    public async Task UploadFileAsync_creates_then_sends_and_returns_the_id()
    {
        //Arrange
        var createPath = ApiEndpoints.FileUploadsApiUrls.Create();
        var sendPath = ApiEndpoints.FileUploadsApiUrls.Send("fu_1");
        Server.Given(CreatePostRequestBuilder(createPath))
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(FileUploadJson("fu_1")));
        Server.Given(CreatePostRequestBuilder(sendPath))
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(FileUploadJson("fu_1")));

        var pic = Path.ChangeExtension(Path.GetTempFileName(), ".png");
        await File.WriteAllBytesAsync(pic, new byte[] { 1, 2, 3 }, TestContext.Current.CancellationToken);

        try
        {
            //Act
            var id = await _client.UploadFileAsync(pic, cancellationToken: TestContext.Current.CancellationToken);

            //Assert
            id.Should().Be("fu_1");
            Server.LogEntries.Count(e => e.Method.Method == "POST" && e.Uri.AbsolutePath == createPath).Should().Be(1);
            Server.LogEntries.Count(e => e.Method.Method == "POST" && e.Uri.AbsolutePath == sendPath).Should().Be(1);
        }
        finally
        {
            File.Delete(pic);
        }
    }

    [Fact]
    public async Task RetrieveAllChildrenAsync_follows_pagination()
    {
        //Arrange — first page has_more with a cursor, second page ends it
        var blockId = "22222222-2222-2222-2222-222222222222";
        var path = ApiEndpoints.BlocksApiUrls.RetrieveChildren(blockId);
        Server.Given(CreateGetRequestBuilder(path)).InScenario("pg").WhenStateIs(null).WillSetStateTo("page2")
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(BlockListJson("b1", hasMore: true, cursor: "c2")));
        Server.Given(CreateGetRequestBuilder(path)).InScenario("pg").WhenStateIs("page2")
            .RespondWith(Response.Create().WithStatusCode(200).WithBody(BlockListJson("b2", hasMore: false, cursor: null)));

        //Act
        var all = await _client.RetrieveAllChildrenAsync(blockId, TestContext.Current.CancellationToken);

        //Assert
        all.Should().HaveCount(2);
        Server.LogEntries.Count(e => e.Method.Method == "GET" && e.Uri.AbsolutePath == path).Should().Be(2);
    }

    private static string PageJson(string id) => "{\"object\":\"page\",\"id\":\"" + id + "\"}";

    private static string FileUploadJson(string id) =>
        "{\"object\":\"file_upload\",\"id\":\"" + id + "\",\"status\":\"uploaded\"}";

    private static string BlockListJson(string blockId, bool hasMore, string cursor) =>
        "{\"object\":\"list\",\"results\":[{\"object\":\"block\",\"id\":\"" + blockId +
        "\",\"type\":\"paragraph\",\"paragraph\":{\"rich_text\":[]}}],\"has_more\":" +
        (hasMore ? "true" : "false") +
        (cursor != null ? ",\"next_cursor\":\"" + cursor + "\"" : string.Empty) + "}";
}
