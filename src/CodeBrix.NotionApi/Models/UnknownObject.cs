using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Fallback deserialization target for <see cref="IObject"/> responses whose
/// <c>"object"</c> discriminator is not yet recognised by the library.
/// Preserves the <c>id</c> and <c>object</c> fields; all other properties are ignored.
/// </summary>
public class UnknownObject : IObject, ISearchResponseObject, IQueryDataSourceResponseObject
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object { get; set; }
}
