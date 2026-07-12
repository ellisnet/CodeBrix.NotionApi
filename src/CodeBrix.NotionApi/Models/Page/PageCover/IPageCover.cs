using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(ExternalPageCover), "external")]
[JsonKnownType(typeof(FilePageCover), "file")]
[JsonFallbackType(typeof(ExternalPageCover))]
public interface IPageCover
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
