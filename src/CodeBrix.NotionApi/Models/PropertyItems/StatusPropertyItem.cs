using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class StatusPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "status";

    [JsonPropertyName("status")]
    public SelectOption Status { get; set; }
}
