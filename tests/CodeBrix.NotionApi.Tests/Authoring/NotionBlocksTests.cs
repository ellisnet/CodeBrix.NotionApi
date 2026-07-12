using System.Collections.Generic;
using System.Linq;
using CodeBrix.NotionApi;
using Xunit;
using SilverAssertions;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class NotionBlocksTests
{
    [Fact]
    public void Paragraph_from_text_sets_rich_text()
    {
        var p = NotionBlocks.Paragraph("hello");
        p.Type.Should().Be(BlockType.Paragraph);
        p.Paragraph.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("hello");
    }

    [Fact]
    public void Heading2_and_Heading3_set_text()
    {
        NotionBlocks.Heading2("h2").Heading_2.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("h2");
        NotionBlocks.Heading3("h3").Heading_3.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("h3");
    }

    [Fact]
    public void Callout_sets_emoji_icon()
    {
        var callout = NotionBlocks.Callout("note", "🚀");
        ((EmojiPageIconRequest)callout.Callout.Icon).Emoji.Should().Be("🚀");
        callout.Callout.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("note");
    }

    [Fact]
    public void Bullet_Numbered_Quote_set_text()
    {
        NotionBlocks.Bullet("b").BulletedListItem.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("b");
        NotionBlocks.Numbered("n").NumberedListItem.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("n");
        NotionBlocks.Quote("q").Quote.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("q");
    }

    [Fact]
    public void Toggle_carries_child_blocks()
    {
        var toggle = NotionBlocks.Toggle("summary", new INonColumnBlockRequest[] { NotionBlocks.Paragraph("child") });
        toggle.Toggle.RichText.Cast<RichTextText>().Single().Text.Content.Should().Be("summary");
        toggle.Toggle.Children.Should().HaveCount(1);
    }

    [Fact]
    public void Divider_is_a_divider()
        => NotionBlocks.Divider().Type.Should().Be(BlockType.Divider);

    [Fact]
    public void Table_infers_width_and_carries_rows()
    {
        var rows = new List<IReadOnlyList<IEnumerable<RichTextText>>>
        {
            new List<IEnumerable<RichTextText>> { NotionText.Split("a"), NotionText.Split("b") },
            new List<IEnumerable<RichTextText>> { NotionText.Split("c"), NotionText.Split("d") }
        };

        var table = NotionBlocks.Table(rows, hasColumnHeader: true);

        table.Table.TableWidth.Should().Be(2);
        table.Table.HasColumnHeader.Should().BeTrue();
        table.Table.Children.Should().HaveCount(2);
    }

    [Fact]
    public void ImageExternal_and_ImageUpload_set_their_source()
    {
        ((ExternalFile)NotionBlocks.ImageExternal("https://x.test/y.png").Image).External.Url.Should().Be("https://x.test/y.png");
        ((NewFileUpload)NotionBlocks.ImageUpload("fu_1").Image).FileUpload.Id.Should().Be("fu_1");
    }
}
