using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(AudioBlock), BlockType.AudioValue)]
[JsonKnownType(typeof(BookmarkBlock), BlockType.BookmarkValue)]
[JsonKnownType(typeof(BreadcrumbBlock), BlockType.BreadcrumbValue)]
[JsonKnownType(typeof(BulletedListItemBlock), BlockType.BulletedListItemValue)]
[JsonKnownType(typeof(CalloutBlock), BlockType.CalloutValue)]
[JsonKnownType(typeof(ChildPageBlock), BlockType.ChildPageValue)]
[JsonKnownType(typeof(ChildDatabaseBlock), BlockType.ChildDatabaseValue)]
[JsonKnownType(typeof(CodeBlock), BlockType.CodeValue)]
[JsonKnownType(typeof(ColumnBlock), BlockType.ColumnValue)]
[JsonKnownType(typeof(ColumnListBlock), BlockType.ColumnListValue)]
[JsonKnownType(typeof(DividerBlock), BlockType.DividerValue)]
[JsonKnownType(typeof(EmbedBlock), BlockType.EmbedValue)]
[JsonKnownType(typeof(EquationBlock), BlockType.EquationValue)]
[JsonKnownType(typeof(FileBlock), BlockType.FileValue)]
[JsonKnownType(typeof(HeadingOneBlock), BlockType.Heading1Value)]
[JsonKnownType(typeof(HeadingTwoBlock), BlockType.Heading2Value)]
[JsonKnownType(typeof(HeadingThreeBlock), BlockType.Heading3Value)]
[JsonKnownType(typeof(HeadingFourBlock), BlockType.Heading4Value)]
[JsonKnownType(typeof(ImageBlock), BlockType.ImageValue)]
[JsonKnownType(typeof(LinkPreviewBlock), BlockType.LinkPreviewValue)]
[JsonKnownType(typeof(LinkToPageBlock), BlockType.LinkToPageValue)]
[JsonKnownType(typeof(NumberedListItemBlock), BlockType.NumberedListItemValue)]
[JsonKnownType(typeof(ParagraphBlock), BlockType.ParagraphValue)]
[JsonKnownType(typeof(PDFBlock), BlockType.PDFValue)]
[JsonKnownType(typeof(QuoteBlock), BlockType.QuoteValue)]
[JsonKnownType(typeof(SyncedBlockBlock), BlockType.SyncedBlockValue)]
[JsonKnownType(typeof(TableBlock), BlockType.TableValue)]
[JsonKnownType(typeof(TableRowBlock), BlockType.TableRowValue)]
[JsonKnownType(typeof(TableOfContentsBlock), BlockType.TableOfContentsValue)]
[JsonKnownType(typeof(TemplateBlock), BlockType.TemplateValue)]
[JsonKnownType(typeof(ToDoBlock), BlockType.ToDoValue)]
[JsonKnownType(typeof(ToggleBlock), BlockType.ToggleValue)]
[JsonKnownType(typeof(VideoBlock), BlockType.VideoValue)]
[JsonKnownType(typeof(MeetingNotesBlock), BlockType.MeetingNotesValue)]
#pragma warning disable CS0618 // "transcription" payloads still deserialize, into the obsolete block
[JsonKnownType(typeof(TranscriptionBlock), BlockType.TranscriptionValue)]
#pragma warning restore CS0618
[JsonKnownType(typeof(TabBlock), BlockType.TabValue)]
[JsonKnownType(typeof(UnsupportedBlock), BlockType.UnsupportedValue)]
[JsonFallbackType(typeof(UnsupportedBlock))]
public interface IBlock : IObject, IObjectModificationData
{
    [JsonPropertyName("type")]
    BlockType Type { get; set; }

    [JsonPropertyName("has_children")]
    bool HasChildren { get; set; }

    [JsonPropertyName("in_trash")]
    bool InTrash { get; set; }

    [JsonPropertyName("parent")]
    IParentOfBlock Parent { get; set; }
}
