using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IPaginationParameters
{
    /// <summary>
    /// If supplied, this endpoint will return a page of results starting after the cursor provided. 
    /// If not supplied, this endpoint will return the first page of results.
    /// </summary>
    [JsonPropertyName("start_cursor")]
    string StartCursor { get; set; }

    /// <summary>
    /// The number of items from the full list desired in the response.
    /// </summary>
    [JsonPropertyName("page_size")]
    int? PageSize { get; set; }
}

public class PaginatedList<T>
{
    [JsonPropertyName("object")]
    public const string Object = "list";

    [JsonPropertyName("results")]
    public List<T> Results { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("next_cursor")]
    public string NextCursor { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
