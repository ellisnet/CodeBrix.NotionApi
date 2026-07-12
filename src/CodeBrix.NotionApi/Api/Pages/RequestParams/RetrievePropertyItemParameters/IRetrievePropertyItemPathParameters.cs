using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IRetrievePropertyItemPathParameters
{
    [JsonPropertyName("page_id")]
    string PageId { get; set; }

    [JsonPropertyName("property_id")]
    string PropertyId { get; set; }
}
