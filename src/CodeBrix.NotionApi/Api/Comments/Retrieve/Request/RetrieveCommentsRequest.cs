using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrieveCommentsRequest : IRetrieveCommentsQueryParameters
{
    [JsonPropertyName("block_id")]
    public string BlockId { get; set; }

    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
