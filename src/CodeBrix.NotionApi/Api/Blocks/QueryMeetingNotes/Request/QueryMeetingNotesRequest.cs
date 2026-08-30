using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class QueryMeetingNotesRequest
{
    /// <summary>
    /// Filter criteria for the meeting notes query.
    /// </summary>
    [JsonPropertyName("filter")]
    public object Filter { get; set; }

    /// <summary>
    /// Sort criteria for the meeting notes query.
    /// </summary>
    [JsonPropertyName("sort")]
    public object Sort { get; set; }

    /// <summary>
    /// Maximum number of results to return. Defaults to 100.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>
    /// Cursor for pagination — pass the value of <c>next_cursor</c> from the previous response.
    /// </summary>
    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    /// <summary>
    /// Number of items per page (max 100).
    /// </summary>
    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
