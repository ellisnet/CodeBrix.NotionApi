using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(SinglePropertyRelation), "single_property")]
[JsonKnownType(typeof(DualPropertyRelation), "dual_property")]
[JsonFallbackType(typeof(SinglePropertyRelation))]
public abstract class RelationData
{
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public virtual RelationType Type { get; set; }
}
