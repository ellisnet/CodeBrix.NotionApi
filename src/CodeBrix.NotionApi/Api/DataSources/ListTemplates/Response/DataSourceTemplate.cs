using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DataSourceTemplate
{
    /// <summary>
    /// The unique identifier of the data source template.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The name of the data source template.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Indicates whether the template is the default template for the data source.
    /// </summary>
    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }
}
