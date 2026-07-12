using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SearchRequest : ISearchBodyParameters
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("sort")]
    public SearchSort Sort { get; set; }

    [JsonPropertyName("filter")]
    public SearchFilter Filter { get; set; }

    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
