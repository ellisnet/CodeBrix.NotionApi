using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Non-generic base interface for update property configuration requests.
/// This allows different property types to be stored in the same collection.
/// </summary>
[JsonConverter(typeof(UpdatePropertyConfigurationRequestConverterFactory))]
public interface IUpdatePropertyConfigurationRequest
{
    [JsonPropertyName("name")]
    string Name { get; set; }
}
