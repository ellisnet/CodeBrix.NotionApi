using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("object")]
[JsonKnownType(typeof(Page), ObjectType.PageValue)]
[JsonKnownType(typeof(DataSource), ObjectType.DataSourceValue)]
[JsonFallbackType(typeof(UnknownObject))]
public interface IQueryDataSourceResponseObject : IObject
{
}
