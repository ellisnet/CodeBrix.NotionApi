using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingTwoBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("heading_2")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_2 { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Heading2;

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
