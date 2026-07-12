using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DualPropertyRelation : RelationData
{
    public override RelationType Type => RelationType.Dual;

    [JsonPropertyName("dual_property")]
    public Data DualProperty { get; set; }

    public class Data
    {
        [JsonPropertyName("synced_property_name")]
        public string SyncedPropertyName { get; set; }

        [JsonPropertyName("synced_property_id")]
        public string SyncedPropertyId { get; set; }
    }
}
