using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(DatabaseParent), "database_id")]
[JsonKnownType(typeof(PageParent), "page_id")]
[JsonKnownType(typeof(WorkspaceParent), "workspace")]
[JsonKnownType(typeof(BlockParent), "block_id")]
[JsonFallbackType(typeof(PageParent))]
public interface IParentOfDatabase
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
