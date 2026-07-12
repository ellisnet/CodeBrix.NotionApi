using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MentionInput
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("user")]
    public Person User { get; set; }

    [JsonPropertyName("page")]
    public ObjectId Page { get; set; }

    [JsonPropertyName("database")]
    public ObjectId Database { get; set; }

    [JsonPropertyName("date")]
    public Date Date { get; set; }
}
