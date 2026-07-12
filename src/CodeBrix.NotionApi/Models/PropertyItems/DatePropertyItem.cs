using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DatePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "date";

    [JsonPropertyName("date")]
    public Date Date { get; set; }
}
