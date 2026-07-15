using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("object")]
[JsonKnownType(typeof(Page), ObjectType.PageValue)]
[JsonKnownType(typeof(Database), ObjectType.DatabaseValue)]
[JsonKnownType(typeof(IBlock), ObjectType.BlockValue)]
[JsonKnownType(typeof(User), ObjectType.UserValue)]
[JsonKnownType(typeof(PageMarkdownResponse), ObjectType.PageMarkdownValue)]
[JsonFallbackType(typeof(UnknownObject))]
public interface IObject
{
    [JsonPropertyName("id")]
    string Id { get; set; }

    [JsonPropertyName("object")]
    ObjectType Object { get; }
}
