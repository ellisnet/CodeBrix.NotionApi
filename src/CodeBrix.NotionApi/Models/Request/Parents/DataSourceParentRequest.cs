using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DataSourceParentRequest : IParentOfPageRequest, IParentOfDataSourceRequest
{
    [JsonPropertyName("type")]
    public string Type => "data_source_id";

    [JsonPropertyName("data_source_id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
