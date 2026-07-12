using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TitlePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "title";

    [JsonPropertyName("title")]
    public RichTextBase Title { get; set; }
}
