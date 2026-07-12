using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RollupProperty : Property
{
    public override PropertyType Type => PropertyType.Rollup;

    [JsonPropertyName("rollup")]
    public Rollup Rollup { get; set; }
}

public class Rollup
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
}
