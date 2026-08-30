using System.Linq;
using System.Text.Json;
using CodeBrix.NotionApi;
using SilverAssertions;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

// Covers the block types and block fields introduced by Notion API version 2026-03-11:
// heading_4, tab, meeting_notes (the renamed transcription block), column width_ratio,
// numbered-list list_start_index / list_format, and the paragraph icon used inside tabs.
public class NewBlockTypeTests
{
    private static T DeserializeBlock<T>(string json) where T : class, IBlock
    {
        var block = JsonSerializer.Deserialize<IBlock>(json, RestClient.DefaultSerializerOptions);

        return Assert.IsType<T>(block);
    }

    [Fact]
    public void Heading_4_payload_deserializes_to_HeadingFourBlock()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "heading_4",
              "heading_4": {
                "rich_text": [ { "type": "text", "text": { "content": "Sub-sub-heading" } } ],
                "color": "blue",
                "is_toggleable": true
              }
            }
            """;

        // Act
        var block = DeserializeBlock<HeadingFourBlock>(Json);

        // Assert
        block.Type.Should().Be(BlockType.Heading4);
        block.Heading_4.Color.Should().Be(Color.Blue);
        block.Heading_4.IsToggleable.Should().BeTrue();
        block.Heading_4.RichText.OfType<RichTextText>().First().Text.Content.Should().Be("Sub-sub-heading");
    }

    [Fact]
    public void HeadingFourBlockRequest_serializes_with_the_heading_4_type()
    {
        // Arrange
        var request = new HeadingFourBlockRequest
        {
            Heading_4 = new HeadingFourBlockRequest.Info
            {
                RichText = [new RichTextText { Text = new Text { Content = "Sub-sub-heading" } }]
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"type\":\"heading_4\"");
        json.Should().Contain("\"heading_4\"");
    }

    [Fact]
    public void HeadingFourUpdateBlock_serializes_the_heading_4_rich_text()
    {
        // Arrange
        IUpdateBlock updateBlock = new HeadingFourUpdateBlock
        {
            Heading_4 = new HeadingFourUpdateBlock.Info
            {
                RichText = [new RichTextTextInput { Text = new Text { Content = "Renamed" } }]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(updateBlock, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"heading_4\"");
        json.Should().Contain("Renamed");
    }

    [Fact]
    public void Tab_payload_deserializes_to_TabBlock()
    {
        // Arrange
        const string Json = """
            { "object": "block", "id": "block-1", "type": "tab", "tab": {} }
            """;

        // Act
        var block = DeserializeBlock<TabBlock>(Json);

        // Assert
        block.Type.Should().Be(BlockType.Tab);
        block.Tab.Should().NotBeNull();
    }

    [Fact]
    public void TabBlockRequest_serializes_its_paragraph_children()
    {
        // Arrange
        var request = new TabBlockRequest
        {
            Tab = new TabBlockRequest.Data
            {
                Children =
                [
                    new ParagraphBlockRequest
                    {
                        Paragraph = new ParagraphBlockRequest.Info
                        {
                            RichText = [new RichTextText { Text = new Text { Content = "First tab" } }],
                            Icon = new EmojiPageIconRequest { Emoji = "\U0001F4C4" }
                        }
                    }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"type\":\"tab\"");
        json.Should().Contain("\"children\"");
        json.Should().Contain("\"icon\"");
    }

    [Fact]
    public void Meeting_notes_payload_deserializes_to_MeetingNotesBlock()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "meeting_notes",
              "meeting_notes": {
                "title": [ { "type": "text", "text": { "content": "Standup" } } ],
                "status": "completed",
                "children": {
                  "summary_block_id": "summary-1",
                  "notes_block_id": "notes-1",
                  "transcript_block_id": "transcript-1"
                },
                "calendar_event": {
                  "start_time": "2026-08-30T09:00:00Z",
                  "end_time": "2026-08-30T09:30:00Z",
                  "attendees": [ "someone@example.com" ]
                },
                "recording": {
                  "start_time": "2026-08-30T09:01:00Z",
                  "end_time": "2026-08-30T09:29:00Z"
                }
              }
            }
            """;

        // Act
        var block = DeserializeBlock<MeetingNotesBlock>(Json);

        // Assert
        block.Type.Should().Be(BlockType.MeetingNotes);
        block.MeetingNotes.Status.Should().Be("completed");
        block.MeetingNotes.Children.SummaryBlockId.Should().Be("summary-1");
        block.MeetingNotes.CalendarEvent.Attendees.Should().ContainSingle();
        block.MeetingNotes.Recording.EndTime.Should().Be("2026-08-30T09:29:00Z");
        block.MeetingNotes.Title.OfType<RichTextText>().First().Text.Content.Should().Be("Standup");
    }

    [Fact]
    public void Legacy_transcription_payload_still_deserializes()
    {
        // Arrange - workspaces pinned to an older Notion-Version still return "transcription"
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "transcription",
              "transcription": { "status": "completed" }
            }
            """;

        // Act
#pragma warning disable CS0618 // deliberately exercising the deprecated block type
        var block = DeserializeBlock<TranscriptionBlock>(Json);

        // Assert
        block.Transcription.Status.Should().Be("completed");
        block.Type.Should().Be(BlockType.Transcription);
#pragma warning restore CS0618
    }

    [Fact]
    public void Column_payload_carries_the_width_ratio()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "column",
              "column": { "width_ratio": 0.25 }
            }
            """;

        // Act
        var block = DeserializeBlock<ColumnBlock>(Json);

        // Assert
        block.Column.WidthRatio.Should().Be(0.25);
    }

    [Fact]
    public void ColumnBlockRequest_serializes_the_width_ratio()
    {
        // Arrange
        var request = new ColumnBlockRequest
        {
            Column = new ColumnBlockRequest.Info { WidthRatio = 0.75 }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"width_ratio\":0.75");
    }

    [Fact]
    public void ColumnBlockRequest_omits_an_unset_width_ratio()
    {
        // Arrange
        var request = new ColumnBlockRequest
        {
            Column = new ColumnBlockRequest.Info()
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().NotContain("width_ratio");
    }

    [Fact]
    public void Numbered_list_item_carries_the_start_index_and_format()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "numbered_list_item",
              "numbered_list_item": {
                "rich_text": [],
                "list_start_index": 5,
                "list_format": "roman"
              }
            }
            """;

        // Act
        var block = DeserializeBlock<NumberedListItemBlock>(Json);

        // Assert
        block.NumberedListItem.ListStartIndex.Should().Be(5);
        block.NumberedListItem.ListFormat.Should().Be(NumberedListFormat.Roman);
    }

    [Fact]
    public void Numbered_list_format_preserves_a_value_notion_adds_later()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "numbered_list_item",
              "numbered_list_item": { "rich_text": [], "list_format": "greek_letters" }
            }
            """;

        // Act
        var block = DeserializeBlock<NumberedListItemBlock>(Json);

        // Assert - extensible enum, so an unknown value round-trips instead of throwing
        block.NumberedListItem.ListFormat.ToString().Should().Be("greek_letters");
    }

    [Fact]
    public void Paragraph_payload_carries_the_tab_icon()
    {
        // Arrange
        const string Json = """
            {
              "object": "block",
              "id": "block-1",
              "type": "paragraph",
              "paragraph": {
                "rich_text": [],
                "icon": { "type": "emoji", "emoji": "📄" }
              }
            }
            """;

        // Act
        var block = DeserializeBlock<ParagraphBlock>(Json);

        // Assert
        var icon = Assert.IsType<EmojiPageIcon>(block.Paragraph.Icon);
        icon.Emoji.Should().Be("\U0001F4C4");
    }
}
