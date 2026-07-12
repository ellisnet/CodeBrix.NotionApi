using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Sort
{
    [JsonPropertyName("property")]
    public string Property { get; set; }

    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Timestamp Timestamp { get; set; }

    [JsonPropertyName("direction")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Direction Direction { get; set; }
}
