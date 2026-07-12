using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("object")]
[JsonKnownType(typeof(SimplePropertyItem), "property_item")]
[JsonKnownType(typeof(ListPropertyItem), "list")]
[JsonFallbackType(typeof(SimplePropertyItem))]
public interface IPropertyItemObject
{
    [JsonPropertyName("object")]
    string Object { get; }

    [JsonPropertyName("type")]
    string Type { get; }

    [JsonPropertyName("id")]
    string Id { get; }

    /// <summary>
    ///     Only present in paginated property values with another page of results. If present, the url the user can request to
    ///     get the next page of results.
    /// </summary>
    [JsonPropertyName("next_url")]
    string NextURL { get; }
}
