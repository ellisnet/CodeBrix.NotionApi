using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(DatabaseParent), ParentTypes.Database)]
[JsonKnownType(typeof(DatasourceParent), ParentTypes.Datasource)]
[JsonKnownType(typeof(PageParent), ParentTypes.Page)]
[JsonKnownType(typeof(BlockParent), ParentTypes.Block)]
[JsonKnownType(typeof(WorkspaceParent), ParentTypes.Workspace)]
[JsonFallbackType(typeof(PageParent))]
public interface IParentOfPage
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
