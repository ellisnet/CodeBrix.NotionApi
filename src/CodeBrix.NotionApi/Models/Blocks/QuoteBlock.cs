using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class QuoteBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("quote")]
    public Info Quote { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Quote;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<INonColumnBlock> Children { get; set; }
    }
}
