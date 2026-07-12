using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingThreeBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("heading_3")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_3 { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Heading3;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("is_toggleable")]
        public bool IsToggleable { get; set; }
    }
}
