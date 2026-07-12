using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(LinkPageToPage), "page_id")]
[JsonKnownType(typeof(LinkDatabaseToPage), "database_id")]
[JsonKnownType(typeof(LinkCommentToPage), "comment_id")]
[JsonFallbackType(typeof(LinkPageToPage))]
public interface ILinkToPage
{
    [JsonPropertyName("type")]
    string Type { get; }
}
