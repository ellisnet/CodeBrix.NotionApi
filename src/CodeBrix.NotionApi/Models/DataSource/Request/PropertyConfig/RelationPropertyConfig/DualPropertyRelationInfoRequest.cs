using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DualPropertyRelationInfoRequest : IRelationInfoRequest
{
    [JsonPropertyName("data_source_id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("type")]
    public string Type => "dual_property";

    [JsonPropertyName("dual_property")]
    public Data DualProperty { get; set; }

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }

    public class Data
    {
        [JsonPropertyName("synced_property_id")]
        public string SyncedPropertyId { get; set; }

        [JsonPropertyName("synced_property_name")]
        public string SyncedPropertyName { get; set; }

        /// <summary>
        /// Additional data for future compatibility
        /// If you encounter properties that are not yet supported, please open an issue on GitHub.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
