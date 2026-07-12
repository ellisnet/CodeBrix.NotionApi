using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RollupDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Rollup;

    [JsonPropertyName("rollup")]
    public RollupResponse Rollup { get; set; }
}

public class RollupResponse
{
    [JsonPropertyName("relation_property_name")]
    public string RelationPropertyName { get; set; }

    [JsonPropertyName("relation_property_id")]
    public string RelationPropertyId { get; set; }

    [JsonPropertyName("rollup_property_name")]
    public string RollupPropertyName { get; set; }

    [JsonPropertyName("rollup_property_id")]
    public string RollupPropertyId { get; set; }

    [JsonPropertyName("function")]
    public string Function { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
