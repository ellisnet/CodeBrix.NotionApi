using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SilverAssertions;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;

[Collection(NotionIntegrationCollection.Name)]
public class PageWithPageParentTests : IntegrationTestBase, IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    private Page _page;

    public PageWithPageParentTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public async ValueTask InitializeAsync()
    {
        var pagesCreateParameters = PagesCreateParametersBuilder
            .Create(new PageParentRequest() { PageId = ParentPageId })
            .AddProperty("title",
                new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextTextInput { Text = new Text { Content = "Test Page Title" } }
                    }
                }).Build();

        _page = await Client.Pages.CreateAsync(pagesCreateParameters);
    }

    public async ValueTask DisposeAsync()
    {
        await Client.Pages.UpdateAsync(_page.Id, new PagesUpdateParameters { InTrash = true });
    }

    [Fact]
    public async Task Update_Title_Of_Page()
    {
        // Arrange
        var updatePage = new PagesUpdateParameters()
        {
            Properties = new Dictionary<string, PropertyValue>
            {
                {
                    "title",
                    new TitlePropertyValue()
                    {
                        Title = new List<RichTextBase>
                        {
                            new RichTextText { Text = new Text() { Content = "Page Title Updated" } }
                        }
                    }
                }
            }
        };

        // Act
        var updatedPage = await Client.Pages.UpdateAsync(_page.Id, updatePage, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var titleProperty = (ListPropertyItem)await Client.Pages.RetrievePagePropertyItemAsync(
            new RetrievePropertyItemParameters
            {
                PageId = updatedPage.Id,
                PropertyId = updatedPage.Properties["title"].Id
            }
        , cancellationToken: TestContext.Current.CancellationToken);

        titleProperty.Results.First().As<TitlePropertyItem>().Title.PlainText.Should().Be("Page Title Updated");
    }

    [Fact]
    public async Task CreateAsync_WithPageEndPosition_CreatesPageSuccessfully()
    {
        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "End Position Page" } }
                    }
                })
                .SetPosition(new PageEndPosition())
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        page.Should().NotBeNull();
        page.Parent.Should().BeOfType<PageParent>()
            .Which.PageId.Should().Be(_page.Id);
    }

    [Fact]
    public async Task CreateAsync_WithPageStartPosition_CreatesPageSuccessfully()
    {
        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "Start Position Page" } }
                    }
                })
                .SetPosition(new PageStartPosition())
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        page.Should().NotBeNull();
        page.Parent.Should().BeOfType<PageParent>()
            .Which.PageId.Should().Be(_page.Id);
    }

    [Fact]
    public async Task CreateAsync_WithAfterBlockPosition_CreatesPageAfterSibling()
    {
        // Create a sibling page first to use as the anchor
        var sibling = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "Sibling Page" } }
                    }
                })
                .SetPosition(new PageEndPosition())
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "After Sibling Page" } }
                    }
                })
                .SetPosition(new AfterBlockPagePosition
                {
                    AfterBlock = new AfterBlockReference { Id = sibling.Id }
                })
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        page.Should().NotBeNull();
        page.Parent.Should().BeOfType<PageParent>()
            .Which.PageId.Should().Be(_page.Id);
    }

    [Fact]
    public async Task CreateAsync_WithNoneTemplate_CreatesPageSuccessfully()
    {
        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "No Template Page" } }
                    }
                })
                .SetTemplate(new NonePageTemplate())
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        page.Should().NotBeNull();
        page.Parent.Should().BeOfType<PageParent>()
            .Which.PageId.Should().Be(_page.Id);
    }

    [Fact]
    public async Task UpdateAsync_WithEraseContent_ClearsPageContent()
    {
        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .SetMarkdown("# Heading\n\nSome content to erase.")
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        var updated = await Client.Pages.UpdateAsync(page.Id, new PagesUpdateParameters
        {
            EraseContent = true
        }, cancellationToken: TestContext.Current.CancellationToken);

        updated.Should().NotBeNull();
        updated.Id.Should().Be(page.Id);

        var markdown = await Client.Pages.RetrieveAsMarkdownAsync(
            new RetrievePageAsMarkdownRequest { PageId = page.Id }, cancellationToken: TestContext.Current.CancellationToken);

        // After erase_content the page markdown should be empty or very minimal
        markdown.Markdown.Should().NotContain("Some content to erase.");
    }

    [Fact]
    public async Task UpdateAsync_WithIsLocked_LocksPage()
    {
        var page = await Client.Pages.CreateAsync(
            PagesCreateParametersBuilder
                .Create(new PageParentRequest { PageId = _page.Id })
                .AddProperty("title", new TitlePropertyValue
                {
                    Title = new List<RichTextBase>
                    {
                        new RichTextText { Text = new Text { Content = "Page To Lock" } }
                    }
                })
                .Build()
        , cancellationToken: TestContext.Current.CancellationToken);

        // Lock the page
        var locked = await Client.Pages.UpdateAsync(page.Id, new PagesUpdateParameters
        {
            IsLocked = true
        }, cancellationToken: TestContext.Current.CancellationToken);

        locked.Should().NotBeNull();

        // Unlock for cleanup
        await Client.Pages.UpdateAsync(page.Id, new PagesUpdateParameters
        {
            IsLocked = false,
            InTrash = true
        }, cancellationToken: TestContext.Current.CancellationToken);
    }
}
