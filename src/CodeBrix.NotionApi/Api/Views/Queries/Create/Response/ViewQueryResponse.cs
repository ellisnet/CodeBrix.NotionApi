using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ViewQueryResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("view_id")]
    public string ViewId { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("results")]
    public IEnumerable<PageReference> Results { get; set; }

    [JsonPropertyName("next_cursor")]
    public string NextCursor { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}
