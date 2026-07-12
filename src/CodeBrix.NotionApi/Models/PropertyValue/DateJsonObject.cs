using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

internal class DateJsonObject
{
    [JsonPropertyName("start")]
    public string Start { get; set; }

    [JsonPropertyName("end")]
    public string End { get; set; }

    [JsonPropertyName("time_zone")]
    public string TimeZone { get; set; }
}
