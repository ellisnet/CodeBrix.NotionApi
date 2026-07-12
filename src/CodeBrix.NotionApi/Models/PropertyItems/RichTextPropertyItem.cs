using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "rich_text";

    [JsonPropertyName("rich_text")]
    public RichTextBase RichText { get; set; }
}
