using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(PageParent), ParentTypes.Page)]
[JsonKnownType(typeof(BlockParent), ParentTypes.Block)]
[JsonFallbackType(typeof(BlockParent))]
public interface IParentOfComment
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
