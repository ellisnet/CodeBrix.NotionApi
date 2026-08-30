using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class QueryDataSourceRequest : IQueryDataSourcePathParameters, IQueryDataSourceQueryParameters, IQueryDataSourceBodyParameters
{
    /// <summary>
    /// The ID of the data source to query.
    /// </summary>
    public string DataSourceId { get; set; }

    [JsonPropertyName("filter_properties")]
    public IEnumerable<string> FilterProperties { get; set; }

    /// <summary>
    /// A list of sort objects to apply to the query results.
    /// </summary>
    [JsonPropertyName("sorts")]
    public IEnumerable<Sort> Sorts { get; set; }

    /// <summary>
    /// A filter object to apply to the query results.
    /// </summary>
    [JsonPropertyName("filter")]
    public Filter Filter { get; set; }

    /// <summary>
    /// When supplied, returns a page of results starting after the cursor provided.
    /// </summary>
    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    /// <summary>
    /// The number of items from the full list desired in the response. Maximum: 100.
    /// </summary>
    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [Obsolete("Use InTrash instead. The 'archived' field is deprecated as of Notion API version 2026-03-11.")]
    [JsonPropertyName("archived")]
    public bool? Archived { get; set; }

    /// <summary>
    /// Whether to include results in trash.
    /// </summary>
    [JsonPropertyName("in_trash")]
    public bool? InTrash { get; set; }

    /// <summary>
    /// Optionally filter the results to only include pages or data sources.
    /// Regular, non-wiki databases only support page children. The default behavior
    /// is no result type filtering, returning both pages and data sources for wikis.
    /// </summary>
    [JsonPropertyName("result_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QueryResultType? ResultType { get; set; }
}
