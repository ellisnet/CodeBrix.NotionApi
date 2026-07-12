using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DataSourceReferenceResponse
{
    [JsonPropertyName("id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
