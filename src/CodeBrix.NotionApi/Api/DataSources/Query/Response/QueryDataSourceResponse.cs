using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class QueryDataSourceResponse : PaginatedList<IQueryDataSourceResponseObject>
{
    [JsonPropertyName("page_or_data_source")]
    public Dictionary<string, object> PageOrDataSource { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
