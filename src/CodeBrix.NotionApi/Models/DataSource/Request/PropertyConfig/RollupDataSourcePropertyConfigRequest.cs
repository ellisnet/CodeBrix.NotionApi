using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RollupDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "rollup";

    [JsonPropertyName("rollup")]
    public RollupOptions Rollup { get; set; }

    public class RollupOptions
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

        /// <summary>
        /// Additional data for future compatibility
        /// If you encounter properties that are not yet supported, please open an issue on GitHub.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
