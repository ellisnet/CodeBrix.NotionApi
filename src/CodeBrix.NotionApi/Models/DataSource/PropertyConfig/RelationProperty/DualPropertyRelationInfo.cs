using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DualPropertyRelationInfo : RelationInfo
{
    public override string Type => "dual_property";

    [JsonPropertyName("dual_property")]
    public Data DualProperty { get; set; }

    public class Data
    {
        [JsonPropertyName("synced_property_name")]
        public string SyncedPropertyName { get; set; }

        [JsonPropertyName("synced_property_id")]
        public string SyncedPropertyId { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
