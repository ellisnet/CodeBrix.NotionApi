using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListDataSourceTemplatesResponse
{
    /// <summary>
    /// A collection of data source templates.
    /// </summary>
    [JsonPropertyName("templates")]
    public IEnumerable<DataSourceTemplate> Templates { get; set; }

    /// <summary>
    /// Indicates whether there are more templates to retrieve.
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// The cursor to use for fetching the next page of templates.
    /// </summary>
    [JsonPropertyName("next_cursor")]
    public string NextCursor { get; set; }
}
