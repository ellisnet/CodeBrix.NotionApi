using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UrlPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "url";

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
