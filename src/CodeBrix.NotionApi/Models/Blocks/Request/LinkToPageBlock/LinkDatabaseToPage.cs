using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LinkDatabaseToPage : ILinkToPage
{
    [JsonPropertyName("type")]
    public string Type => "database_id";

    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }
}
