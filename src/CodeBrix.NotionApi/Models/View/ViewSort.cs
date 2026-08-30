using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ViewSort
{
    [JsonPropertyName("property")]
    public string Property { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [JsonPropertyName("direction")]
    public string Direction { get; set; }
}
