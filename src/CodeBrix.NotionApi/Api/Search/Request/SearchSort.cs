using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SearchSort
{
    /// <summary>
    /// Supported direction values are "ascending" and "descending".
    /// </summary>
    [JsonPropertyName("direction")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SearchDirection Direction { get; set; }

    /// <summary>
    /// The only supported timestamp value is "last_edited_time".
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }
}
