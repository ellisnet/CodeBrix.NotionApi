using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ParagraphBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("paragraph")]
    public Info Paragraph { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Paragraph;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<INonColumnBlock> Children { get; set; }

        /// <summary>
        /// Optional icon for paragraph blocks that are direct children of a tab block.
        /// Setting an icon on other paragraphs results in a validation error from the API.
        /// </summary>
        [JsonPropertyName("icon")]
        public IPageIcon Icon { get; set; }
    }
}
