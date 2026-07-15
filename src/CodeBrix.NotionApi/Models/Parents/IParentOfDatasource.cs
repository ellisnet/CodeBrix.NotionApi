using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(DatabaseParent), "database_id")]
[JsonKnownType(typeof(DatasourceParent), "data_source_id")]
[JsonFallbackType(typeof(DatabaseParent))]
public interface IParentOfDatasource
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
