using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PageParentOfDatabaseRequest : IParentOfDatabaseRequest
{
    [JsonPropertyName("type")]
    public string Type => "page_id";

    [JsonPropertyName("page_id")]
    public string PageId { get; set; }
}
