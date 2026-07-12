using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DatabaseParent : IParentOfDatasource, IParentOfDatabase, IParentOfBlock, IParentOfPage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = ParentTypes.Database;

    [JsonPropertyName(ParentTypes.Database)]
    public string DatabaseId { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
