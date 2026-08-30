using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IQueryDataSourceBodyParameters : IPaginationParameters
{
    /// <summary>
    /// A list of sort objects to apply to the query results.
    /// </summary>
    [JsonPropertyName("sorts")]
    IEnumerable<Sort> Sorts { get; set; }

    /// <summary>
    /// A filter object to apply to the query results.
    /// </summary>
    [JsonPropertyName("filter")]
    Filter Filter { get; set; }

    [Obsolete("Use InTrash instead. The 'archived' field is deprecated as of Notion API version 2026-03-11.")]
    [JsonPropertyName("archived")]
    bool? Archived { get; set; }

    /// <summary>
    /// Whether to include results in trash.
    /// </summary>
    [JsonPropertyName("in_trash")]
    bool? InTrash { get; set; }

    /// <summary>
    /// Optionally filter the results to only include pages or data sources.
    /// </summary>
    [JsonPropertyName("result_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    QueryResultType? ResultType { get; set; }
}
