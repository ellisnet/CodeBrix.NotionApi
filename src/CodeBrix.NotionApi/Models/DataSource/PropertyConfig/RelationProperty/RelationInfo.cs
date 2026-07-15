using System.Collections.Generic;
using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(SinglePropertyRelationInfo), "single_property")]
[JsonKnownType(typeof(DualPropertyRelationInfo), "dual_property")]
[JsonFallbackType(typeof(SinglePropertyRelationInfo))]
public abstract class RelationInfo
{
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }

    [JsonPropertyName("data_source_id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
