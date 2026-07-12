using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ParagraphBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
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
        public IEnumerable<INonColumnBlockRequest> Children { get; set; }
    }
}
