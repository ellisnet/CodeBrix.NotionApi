using System.Collections.Generic;
using System.Linq;

namespace CodeBrix.NotionApi;

/// <summary>
/// Terse factory methods for the block-request types, replacing the verbose
/// <c>new XBlockRequest { X = new XBlockRequest.Info { RichText = ... } }</c> nested-<c>Info</c> pattern.
/// The <c>string</c> overloads treat their argument as LITERAL text (no markdown is interpreted); build
/// bold/italic/code/linked runs explicitly with <see cref="NotionText"/> when you need annotations.
/// </summary>
public static class NotionBlocks
{
    /// <summary>A paragraph block from ready-made rich-text runs.</summary>
    public static ParagraphBlockRequest Paragraph(IEnumerable<RichTextBase> richText, Color? color = null) =>
        new() { Paragraph = new ParagraphBlockRequest.Info { RichText = richText, Color = color } };

    /// <summary>A paragraph block from literal text.</summary>
    public static ParagraphBlockRequest Paragraph(string text, Color? color = null) =>
        Paragraph(NotionText.PlainBase(text), color);

    /// <summary>A heading_2 block from literal text.</summary>
    public static HeadingTwoBlockRequest Heading2(string text) =>
        new() { Heading_2 = new HeadingTwoBlockRequest.Info { RichText = NotionText.PlainBase(text) } };

    /// <summary>A heading_3 block from literal text.</summary>
    public static HeadingThreeBlockRequest Heading3(string text) =>
        new() { Heading_3 = new HeadingThreeBlockRequest.Info { RichText = NotionText.PlainBase(text) } };

    /// <summary>A callout block with an optional emoji icon and optional child blocks.</summary>
    public static CalloutBlockRequest Callout(
        IEnumerable<RichTextBase> richText,
        string emoji = null,
        IEnumerable<INonColumnBlockRequest> children = null,
        Color? color = null)
    {
        var info = new CalloutBlockRequest.Info { RichText = richText, Color = color, Children = children };
        if (!string.IsNullOrEmpty(emoji))
        {
            info.Icon = new EmojiPageIconRequest { Emoji = emoji };
        }

        return new CalloutBlockRequest { Callout = info };
    }

    /// <summary>A callout block from literal text, with an optional emoji icon.</summary>
    public static CalloutBlockRequest Callout(string text, string emoji = null, Color? color = null) =>
        Callout(NotionText.PlainBase(text), emoji, null, color);

    /// <summary>A quote block from ready-made rich-text runs.</summary>
    public static QuoteBlockRequest Quote(IEnumerable<RichTextBase> richText) =>
        new() { Quote = new QuoteBlockRequest.Info { RichText = richText } };

    /// <summary>A quote block from literal text.</summary>
    public static QuoteBlockRequest Quote(string text) => Quote(NotionText.PlainBase(text));

    /// <summary>A bulleted_list_item block from literal text.</summary>
    public static BulletedListItemBlockRequest Bullet(string text) =>
        new() { BulletedListItem = new BulletedListItemBlockRequest.Info { RichText = NotionText.PlainBase(text) } };

    /// <summary>A numbered_list_item block from literal text.</summary>
    public static NumberedListItemBlockRequest Numbered(string text) =>
        new() { NumberedListItem = new NumberedListItemBlockRequest.Info { RichText = NotionText.PlainBase(text) } };

    /// <summary>A toggle block with a summary and child blocks.</summary>
    public static ToggleBlockRequest Toggle(IEnumerable<RichTextBase> summary, IEnumerable<INonColumnBlockRequest> children) =>
        new() { Toggle = new ToggleBlockRequest.Info { RichText = summary, Children = children } };

    /// <summary>A toggle block from literal summary text and child blocks.</summary>
    public static ToggleBlockRequest Toggle(string summary, IEnumerable<INonColumnBlockRequest> children) =>
        Toggle(NotionText.PlainBase(summary), children);

    /// <summary>A divider block.</summary>
    public static DividerBlockRequest Divider() => new() { Divider = new DividerBlockRequest.Data() };

    /// <summary>A single table row from its cells (each cell is a list of rich-text runs).</summary>
    public static TableRowBlockRequest TableRow(IEnumerable<IEnumerable<RichTextText>> cells) =>
        new() { TableRow = new TableRowBlockRequest.Info { Cells = cells } };

    /// <summary>
    /// A table block. Each row is a list of cells; each cell is a list of rich-text runs. The table width
    /// is taken from the first row.
    /// </summary>
    public static TableBlockRequest Table(
        IReadOnlyList<IReadOnlyList<IEnumerable<RichTextText>>> rows,
        bool hasColumnHeader = false,
        bool hasRowHeader = false)
    {
        var width = rows.Count > 0 ? rows[0].Count : 1;
        var rowBlocks = rows.Select(r => TableRow(r)).ToList();
        return new TableBlockRequest
        {
            Table = new TableBlockRequest.Info
            {
                TableWidth = width,
                HasColumnHeader = hasColumnHeader,
                HasRowHeader = hasRowHeader,
                Children = rowBlocks
            }
        };
    }

    /// <summary>An image block referencing an externally-hosted URL, with an optional caption.</summary>
    public static ImageBlockRequest ImageExternal(string url, IEnumerable<RichTextBase> caption = null) =>
        new() { Image = new ExternalFile { External = new ExternalFile.Info { Url = url }, Caption = caption } };

    /// <summary>
    /// An image block referencing an uploaded file by its <c>file_upload</c> id (see
    /// <see cref="NotionAuthoringExtensions.UploadFileAsync"/>), with an optional caption.
    /// </summary>
    public static ImageBlockRequest ImageUpload(string fileUploadId, IEnumerable<RichTextBase> caption = null) =>
        new() { Image = new NewFileUpload { FileUpload = new NewFileUpload.Info { Id = fileUploadId }, Caption = caption } };
}
