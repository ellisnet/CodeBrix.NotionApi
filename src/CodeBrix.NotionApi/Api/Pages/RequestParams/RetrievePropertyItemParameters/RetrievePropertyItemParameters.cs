using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrievePropertyItemParameters : IRetrievePropertyItemPathParameters, IRetrievePropertyQueryParameters
{
    [JsonPropertyName("page_id")]
    public string PageId { get; set; }

    [JsonPropertyName("property_id")]
    public string PropertyId { get; set; }

    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
