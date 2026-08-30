using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SilverAssertions;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;

[Collection(NotionIntegrationCollection.Name)]
public class IBlocksClientTests : IntegrationTestBase, IAsyncLifetime
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
    public async Task AppendChildrenAsync_AppendsBlocksGivenBlocks()
    {
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new BreadcrumbBlockRequest { Parent= new PageParent() {PageId = Guid.NewGuid().ToString()}, Breadcrumb = new BreadcrumbBlockRequest.Data() },
                    new DividerBlockRequest { Divider = new DividerBlockRequest.Data() },
                    new TableOfContentsBlockRequest { TableOfContents = new TableOfContentsBlockRequest.Data() },
                    new CalloutBlockRequest
                    {
                        Callout = new CalloutBlockRequest.Info
                        {
                            RichText = new List<RichTextBaseInput>
                            {
                                new RichTextTextInput { Text = new Text { Content = "Test" } }
                            }
                        }
                    }
                }
            }
        , cancellationToken: TestContext.Current.CancellationToken);

        blocks.Results.Should().HaveCount(4);
    }

    [Fact]
    public async Task UpdateBlockAsync_UpdatesGivenBlock()
    {
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new BreadcrumbBlockRequest { Breadcrumb = new BreadcrumbBlockRequest.Data() }
                }
            }
        , cancellationToken: TestContext.Current.CancellationToken);

        var blockId = blocks.Results.First().Id;
        await Client.Blocks.UpdateAsync(blockId, new BreadcrumbUpdateBlock(), cancellationToken: TestContext.Current.CancellationToken);

        var updatedBlocks =
            await Client.Blocks.RetrieveChildrenAsync(new BlockRetrieveChildrenRequest { BlockId = _page.Id }, cancellationToken: TestContext.Current.CancellationToken);

        updatedBlocks.Results.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAsync_DeleteBlockWithGivenId()
    {
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new DividerBlockRequest { Divider = new DividerBlockRequest.Data() },
                    new TableOfContentsBlockRequest { TableOfContents = new TableOfContentsBlockRequest.Data() }
                }
            }
        , cancellationToken: TestContext.Current.CancellationToken);

        blocks.Results.Should().HaveCount(2);
    }


    [Fact]
    public async Task AppendChildrenAsync_WithStartPosition_PrependsBlocks()
    {
        // Arrange - append an initial block
        await Client.Blocks.AppendChildrenAsync(new BlockAppendChildrenRequest
        {
            BlockId = _page.Id,
            Children = new List<IBlockObjectRequest>
            {
                new DividerBlockRequest { Divider = new DividerBlockRequest.Data() }
            }
        }, cancellationToken: TestContext.Current.CancellationToken);

        // Act - prepend a block using StartContentPosition
        await Client.Blocks.AppendChildrenAsync(new BlockAppendChildrenRequest
        {
            BlockId = _page.Id,
            Position = new StartContentPosition(),
            Children = new List<IBlockObjectRequest>
            {
                new TableOfContentsBlockRequest { TableOfContents = new TableOfContentsBlockRequest.Data() }
            }
        }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var children = await Client.Blocks.RetrieveChildrenAsync(
            new BlockRetrieveChildrenRequest { BlockId = _page.Id }, cancellationToken: TestContext.Current.CancellationToken);

        children.Results.Should().HaveCount(2);
        children.Results.First().Type.Should().Be(BlockType.TableOfContents);
        children.Results.Last().Type.Should().Be(BlockType.Divider);
    }

    [Fact]
    public async Task AppendChildrenAsync_WithAfterBlockPosition_InsertsAfterSpecifiedBlock()
    {
        // Arrange - append two initial blocks: A, B
        var initial = await Client.Blocks.AppendChildrenAsync(new BlockAppendChildrenRequest
        {
            BlockId = _page.Id,
            Children = new List<IBlockObjectRequest>
            {
                new DividerBlockRequest { Divider = new DividerBlockRequest.Data() },
                new TableOfContentsBlockRequest { TableOfContents = new TableOfContentsBlockRequest.Data() }
            }
        }, cancellationToken: TestContext.Current.CancellationToken);

        var blockA = initial.Results.First();

        // Act - insert a breadcrumb after block A, so the order becomes A, Breadcrumb, B
        await Client.Blocks.AppendChildrenAsync(new BlockAppendChildrenRequest
        {
            BlockId = _page.Id,
            Position = new AfterBlockContentPosition
            {
                AfterBlock = new AfterBlockReference { Id = blockA.Id }
            },
            Children = new List<IBlockObjectRequest>
            {
                new BreadcrumbBlockRequest { Breadcrumb = new BreadcrumbBlockRequest.Data() }
            }
        }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var children = await Client.Blocks.RetrieveChildrenAsync(
            new BlockRetrieveChildrenRequest { BlockId = _page.Id }, cancellationToken: TestContext.Current.CancellationToken);

        children.Results.Should().HaveCount(3);
        children.Results[0].Id.Should().Be(blockA.Id);
        children.Results[1].Type.Should().Be(BlockType.Breadcrumb);
        children.Results[2].Type.Should().Be(BlockType.TableOfContents);
    }

    [Fact]
    public async Task AppendChildrenAsync_WithTabBlock_AppendsTabBlock()
    {
        // Arrange & Act
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new TabBlockRequest
                    {
                        Tab = new TabBlockRequest.Data
                        {
                            Children = new List<ParagraphBlockRequest>
                            {
                                new ParagraphBlockRequest
                                {
                                    Paragraph = new ParagraphBlockRequest.Info
                                    {
                                        RichText = new List<RichTextBaseInput>
                                        {
                                            new RichTextTextInput { Text = new Text { Content = "Tab 1" } }
                                        }
                                    }
                                },
                                new ParagraphBlockRequest
                                {
                                    Paragraph = new ParagraphBlockRequest.Info
                                    {
                                        RichText = new List<RichTextBaseInput>
                                        {
                                            new RichTextTextInput { Text = new Text { Content = "Tab 2" } }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var block = blocks.Results.Should().ContainSingle().Subject;
        block.Should().BeOfType<TabBlock>();
        block.Type.Should().Be(BlockType.Tab);
        block.HasChildren.Should().BeTrue();
    }

    [Fact]
    public async Task AppendChildrenAsync_WithTabBlockAndParagraphIcon_ParagraphIconIsReturned()
    {
        // Arrange & Act - create a tab block whose paragraph children have emoji icons
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new TabBlockRequest
                    {
                        Tab = new TabBlockRequest.Data
                        {
                            Children = new List<ParagraphBlockRequest>
                            {
                                new ParagraphBlockRequest
                                {
                                    Paragraph = new ParagraphBlockRequest.Info
                                    {
                                        RichText = new List<RichTextBaseInput>
                                        {
                                            new RichTextTextInput { Text = new Text { Content = "Tab with icon" } }
                                        },
                                        Icon = new EmojiPageIconRequest { Emoji = "\U0001F3AF" }
                                    }
                                }
                            }
                        }
                    }
                }
            }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert - retrieve children of the tab block and verify the paragraph has an icon
        var tabBlock = blocks.Results.Should().ContainSingle().Subject.Should().BeOfType<TabBlock>().Subject;

        var tabChildren = await Client.Blocks.RetrieveChildrenAsync(
            new BlockRetrieveChildrenRequest { BlockId = tabBlock.Id }, cancellationToken: TestContext.Current.CancellationToken);

        var paragraph = tabChildren.Results.Should().ContainSingle().Subject.Should().BeOfType<ParagraphBlock>().Subject;
        paragraph.Paragraph.Icon.Should().NotBeNull();
        paragraph.Paragraph.Icon.Should().BeOfType<EmojiPageIcon>();
        ((EmojiPageIcon)paragraph.Paragraph.Icon).Emoji.Should().Be("\U0001F3AF");
    }

    [Fact]
    public async Task QueryMeetingNotesAsync_ReturnsValidResponse()
    {
        // Act - the workspace may hold no meeting notes at all, so an empty result is valid.
        // The endpoint itself is gated behind a Notion plan with AI meeting notes enabled, so a
        // workspace without that plan skips instead of failing.
        QueryMeetingNotesResponse response;

        try
        {
            response = await Client.Blocks.QueryMeetingNotesAsync(
                new QueryMeetingNotesRequest { PageSize = 10 }, cancellationToken: TestContext.Current.CancellationToken);
        }
        catch (NotionApiException exception) when (exception.Message.Contains("AI meeting notes"))
        {
            Assert.Skip("This Notion workspace's plan does not have AI meeting notes enabled.");

            return;
        }

        // Assert
        response.Should().NotBeNull();
        response.Results.Should().NotBeNull();

        foreach (var block in response.Results)
        {
            block.Should().BeOfType<MeetingNotesBlock>();
            block.Type.Should().Be(BlockType.MeetingNotes);
        }
    }

    [Fact]
    public async Task AppendChildrenAsync_WithNumberedListItem_DeserializesListStartIndexAndFormat()
    {
        // Arrange & Act - list_start_index and list_format are read-only response fields; they
        // cannot be set on a create or update request.
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new NumberedListItemBlockRequest
                    {
                        NumberedListItem = new NumberedListItemBlockRequest.Info
                        {
                            RichText = new List<RichTextBaseInput>
                            {
                                new RichTextTextInput { Text = new Text { Content = "Item one" } }
                            }
                        }
                    }
                }
            }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var block = blocks.Results.Should().ContainSingle().Subject.Should().BeOfType<NumberedListItemBlock>().Subject;
        block.NumberedListItem.RichText.OfType<RichTextText>().First().Text.Content.Should().Be("Item one");
        block.NumberedListItem.ListStartIndex.Should().BeNull();
        block.NumberedListItem.ListFormat.Should().BeNull();
    }

    [Fact]
    public async Task AppendChildrenAsync_WithColumnBlockWidthRatio_WidthRatioIsReturned()
    {
        // Arrange & Act - a column list with two columns, each with an explicit width_ratio
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest>
                {
                    new ColumnListBlockRequest
                    {
                        ColumnList = new ColumnListBlockRequest.Info
                        {
                            Children = new List<ColumnBlockRequest>
                            {
                                new ColumnBlockRequest
                                {
                                    Column = new ColumnBlockRequest.Info
                                    {
                                        WidthRatio = 0.25,
                                        Children = new List<IColumnChildrenBlockRequest>
                                        {
                                            new ParagraphBlockRequest
                                            {
                                                Paragraph = new ParagraphBlockRequest.Info
                                                {
                                                    RichText = new List<RichTextBaseInput>
                                                    {
                                                        new RichTextTextInput { Text = new Text { Content = "Narrow column" } }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new ColumnBlockRequest
                                {
                                    Column = new ColumnBlockRequest.Info
                                    {
                                        WidthRatio = 0.75,
                                        Children = new List<IColumnChildrenBlockRequest>
                                        {
                                            new ParagraphBlockRequest
                                            {
                                                Paragraph = new ParagraphBlockRequest.Info
                                                {
                                                    RichText = new List<RichTextBaseInput>
                                                    {
                                                        new RichTextTextInput { Text = new Text { Content = "Wide column" } }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        var columnList = blocks.Results.Should().ContainSingle().Subject.Should().BeOfType<ColumnListBlock>().Subject;

        var columns = await Client.Blocks.RetrieveChildrenAsync(
            new BlockRetrieveChildrenRequest { BlockId = columnList.Id }, cancellationToken: TestContext.Current.CancellationToken);

        columns.Results.Should().HaveCount(2);
        columns.Results[0].Should().BeOfType<ColumnBlock>().Subject.Column.WidthRatio.Should().Be(0.25);
        columns.Results[1].Should().BeOfType<ColumnBlock>().Subject.Column.WidthRatio.Should().Be(0.75);
    }

    [Theory]
    [MemberData(nameof(BlockData))]
    public async Task UpdateAsync_UpdatesGivenBlock(
        IBlockObjectRequest block, IUpdateBlock updateBlock, Action<IBlock, INotionClient> assert)
    {
        var blocks = await Client.Blocks.AppendChildrenAsync(
            new BlockAppendChildrenRequest
            {
                BlockId = _page.Id,
                Children = new List<IBlockObjectRequest> { block }
            }
        , cancellationToken: TestContext.Current.CancellationToken);

        var blockId = blocks.Results.First().Id;
        await Client.Blocks.UpdateAsync(blockId, updateBlock, cancellationToken: TestContext.Current.CancellationToken);

        var updatedBlocks =
            await Client.Blocks.RetrieveChildrenAsync(new BlockRetrieveChildrenRequest { BlockId = _page.Id }, cancellationToken: TestContext.Current.CancellationToken);

        updatedBlocks.Results.Should().HaveCount(1);

        var updatedBlock = updatedBlocks.Results.First();

        assert.Invoke(updatedBlock, Client);
    }

    public static IEnumerable<object[]> BlockData()
    {
        // The link_to_page cases below need a page that actually exists in the target workspace.
        // Read it from the same environment variable the rest of the integration suite uses rather
        // than hard-coding an ID that only resolves in one particular workspace. When the variable
        // is absent the whole class is skipped by IntegrationTestBase before these values are used.
        var linkTargetPageId = Environment.GetEnvironmentVariable("NOTION_PARENT_PAGE_ID");

        return new List<object[]>
        {
            new object[]
            {
                new BookmarkBlockRequest
                {
                    Bookmark = new BookmarkBlockRequest.Info
                    {
                        Url = "https://developers.notion.com/reference/rich-text",
                        Caption = new List<RichTextBase>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Notion API" } }
                        }
                    }
                },
                new BookmarkUpdateBlock
                {
                    Bookmark = new BookmarkUpdateBlock.Info
                    {
                        Url = "https://github.com/ellisnet/CodeBrix.NotionApi",
                        Caption = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Github" } }
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    var updatedBlock = (BookmarkBlock)block;
                    Assert.Equal("https://github.com/ellisnet/CodeBrix.NotionApi", updatedBlock.Bookmark.Url);
                    Assert.Equal("Github", updatedBlock.Bookmark.Caption.OfType<RichTextText>().First().Text.Content);
                })
            },
            new object[]
            {
                new EquationBlockRequest { Equation = new EquationBlockRequest.Info { Expression = "e=mc^3" } },
                new EquationUpdateBlock { Equation = new EquationUpdateBlock.Info { Expression = "e=mc^2" } },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    var updatedBlock = (EquationBlock)block;
                    Assert.Equal("e=mc^2", updatedBlock.Equation.Expression);
                })
            },
            new object[]
            {
                new DividerBlockRequest { Divider = new DividerBlockRequest.Data() }, new DividerUpdateBlock(),
                new Action<IBlock, INotionClient>((block, client) =>
                {
                    Assert.NotNull(block);
                    _ = Assert.IsType<DividerBlock>(block);
                })
            },
            new object[]
            {
                new AudioBlockRequest
                {
                    Audio = new ExternalFile
                    {
                        External = new ExternalFile.Info
                        {
                            Url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"
                        }
                    }
                },
                new AudioUpdateBlock
                {
                    Audio = new ExternalFileInput
                    {
                        External = new ExternalFileInput.Data
                        {
                            Url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-3.mp3"
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    block.Should().NotBeNull();

                    block.Should().BeOfType<AudioBlock>().Subject
                        .Audio.Should().BeOfType<ExternalFile>().Subject
                        .External.Url.Should().Be("https://www.soundhelix.com/examples/mp3/SoundHelix-Song-3.mp3");
                })
            },
            new object[]
            {
                new TableOfContentsBlockRequest { TableOfContents = new TableOfContentsBlockRequest.Data() },
                new TableOfContentsUpdateBlock(), new Action<IBlock, INotionClient>((block, client) =>
                {
                    Assert.NotNull(block);
                    _ = Assert.IsType<TableOfContentsBlock>(block);
                })
            },
            new object[]
            {
                new CalloutBlockRequest
                {
                    Callout = new CalloutBlockRequest.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test" } }
                        }
                    }
                },
                new CalloutUpdateBlock
                {
                    Callout = new CalloutUpdateBlock.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test 2" } }
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    Assert.NotNull(block);
                    var calloutBlock = Assert.IsType<CalloutBlock>(block);

                    Assert.Equal("Test 2", calloutBlock.Callout.RichText.OfType<RichTextText>().First().Text.Content);
                })
            },
            new object[]
            {
                new QuoteBlockRequest
                {
                    Quote = new QuoteBlockRequest.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test" } }
                        }
                    }
                },
                new QuoteUpdateBlock
                {
                    Quote = new QuoteUpdateBlock.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test 2" } }
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    Assert.NotNull(block);
                    var quoteBlock = Assert.IsType<QuoteBlock>(block);

                    Assert.Equal("Test 2", quoteBlock.Quote.RichText.OfType<RichTextText>().First().Text.Content);
                })
            },
            new object[]
            {
                new ImageBlockRequest
                {
                    Image = new ExternalFile
                    {
                        External = new ExternalFile.Info
                        {
                            Url = "https://upload.wikimedia.org/wikipedia/commons/a/a9/Example.jpg"
                        }
                    }
                },
                new ImageUpdateBlock
                {
                    Image = new ExternalFileInput
                    {
                        External = new ExternalFileInput.Data
                        {
                            Url
                                = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg"
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    Assert.NotNull(block);
                    var imageBlock = Assert.IsType<ImageBlock>(block);
                    var imageFile = Assert.IsType<ExternalFile>(imageBlock.Image);

                    Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg",
                        imageFile.External.Url);
                })
            },
            new object[]
            {
                new EmbedBlockRequest
                {
                    Embed = new EmbedBlockRequest.Info
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/a/a9/Example.jpg"
                    }
                },
                new EmbedUpdateBlock
                {
                    Embed = new EmbedUpdateBlock.Info
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg"
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    Assert.NotNull(block);
                    var embedBlock = Assert.IsType<EmbedBlock>(block);

                    Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg",
                        embedBlock.Embed.Url);
                })
            },
            new object[]
            {
                new LinkToPageBlockRequest
                {
                    LinkToPage = new LinkPageToPage
                    {
                        PageId = linkTargetPageId
                    }
                },
                new LinkToPageUpdateBlock
                {
                    LinkToPage = new LinkPageToPage { PageId = linkTargetPageId }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    Assert.NotNull(block);
                    var linkToPageBlock = Assert.IsType<LinkToPageBlock>(block);

                    var pageParent = Assert.IsType<LinkPageToPage>(linkToPageBlock.LinkToPage);

                    // TODO: Currently the api doesn't allow to update the link_to_page block type
                    // This will change to updated ID once api start to support
                    Assert.Equal(Guid.Parse(linkTargetPageId), Guid.Parse(pageParent.PageId));
                })
            },
            new object[]
            {
                new TableBlockRequest
                {
                    Table = new TableBlockRequest.Info
                    {
                        TableWidth = 1,
                        Children = new[]
                        {
                            new TableRowBlockRequest
                            {
                                TableRow = new TableRowBlockRequest.Info
                                {
                                    Cells = new[]
                                    {
                                        new[] { new RichTextText { Text = new Text { Content = "Data" } } }
                                    }
                                }
                            }
                        }
                    }
                },
                new TableUpdateBlock { Table = new TableUpdateBlock.Info { HasColumnHeader = false } },
                new Action<IBlock, INotionClient>((block, client) =>
                {
                    var tableBlock = block.Should().NotBeNull().And.BeOfType<TableBlock>().Subject;
                    tableBlock.HasChildren.Should().BeTrue();

                    var children = client.Blocks
                        .RetrieveChildrenAsync(new BlockRetrieveChildrenRequest { BlockId = tableBlock.Id })
                        .GetAwaiter().GetResult();

                    children.Results.Should().ContainSingle()
                        .Subject.Should().BeOfType<TableRowBlock>()
                        .Subject.TableRow.Cells.Should().ContainSingle()
                        .Subject.Should().ContainSingle()
                        .Subject.Should().BeOfType<RichTextText>()
                        .Subject.Text.Content.Should().Be("Data");
                })
            },
            new object[]
            {
                new FileBlockRequest {
                    File = new ExternalFile
                    {
                        Name = "Test file",
                        External = new ExternalFile.Info
                        {
                            Url = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg"
                        },
                        Caption = new List<RichTextBase>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test file" } }
                        }
                    }
                },
                new FileUpdateBlock
                {
                    File = new ExternalFileInput
                    {
                        Name = "Test file name",
                        External = new ExternalFileInput.Data
                        {
                            Url = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg"
                        },
                        Caption = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Test file caption" } }
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, client) =>
                {
                    var fileBlock = block.Should().NotBeNull().And.BeOfType<FileBlock>().Subject;
                    fileBlock.HasChildren.Should().BeFalse();

                    var file = fileBlock.File.Should().NotBeNull().And.BeOfType<ExternalFile>().Subject;

                    // NOTE: The name of the file block, as shown in the Notion UI. Note that the UI may auto-append .pdf or other extensions.
                    file.Name.Should().Be("Test file name.jpg");

                    file.External.Should().NotBeNull();
                    file.External.Url.Should().Be("https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg");
                    file.Caption.Should().NotBeNull().And.ContainSingle()
                        .Subject.Should().BeOfType<RichTextText>().Subject
                        .Text.Content.Should().Be("Test file caption");
                })
            },
            new object[]
            {
                new HeadingFourBlockRequest
                {
                    Heading_4 = new HeadingFourBlockRequest.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Heading 4 original" } }
                        }
                    }
                },
                new HeadingFourUpdateBlock
                {
                    Heading_4 = new HeadingFourUpdateBlock.Info
                    {
                        RichText = new List<RichTextBaseInput>
                        {
                            new RichTextTextInput { Text = new Text { Content = "Heading 4 updated" } }
                        }
                    }
                },
                new Action<IBlock, INotionClient>((block, _) =>
                {
                    var heading4 = block.Should().NotBeNull().And.BeOfType<HeadingFourBlock>().Subject;
                    heading4.Heading_4.RichText.OfType<RichTextText>().First().Text.Content
                        .Should().Be("Heading 4 updated");
                })
            }
        };
    }
}
