using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingOneBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("heading_1")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_1 { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Heading1;

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
