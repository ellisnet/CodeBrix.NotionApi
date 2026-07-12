using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ParentPageInput
{
    [JsonPropertyName("page_id")]
    public string PageId { get; set; }
}
