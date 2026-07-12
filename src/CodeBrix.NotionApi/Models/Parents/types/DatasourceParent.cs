using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DatasourceParent : IParentOfDatasource, IParentOfBlock, IParentOfPage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = ParentTypes.Datasource;

    [JsonPropertyName(ParentTypes.Datasource)]
    public string DataSourceId { get; set; }

    [JsonPropertyName(ParentTypes.Database)]
    public string DatabaseId { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
