using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CalloutBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("callout")]
    public Info Callout { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Callout;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("icon")]
        public IPageIcon Icon { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<INonColumnBlock> Children { get; set; }
    }
}
