using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IQueryDataSourceQueryParameters
{
    /// <summary>
    /// List of properties to filter the results by.
    /// </summary>
    [JsonPropertyName("filter_properties")]
    IEnumerable<string> FilterProperties { get; set; }
}
