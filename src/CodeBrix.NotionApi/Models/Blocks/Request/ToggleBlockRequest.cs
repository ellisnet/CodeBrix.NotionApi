using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ToggleBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("toggle")]
    public Info Toggle { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Toggle;

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
