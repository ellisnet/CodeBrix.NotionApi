using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(UserOwner), "user")]
[JsonKnownType(typeof(WorkspaceIntegrationOwner), "workspace")]
[JsonFallbackType(typeof(WorkspaceIntegrationOwner))]
public interface IBotOwner
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
