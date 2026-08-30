using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

// ABSTRACT ON PURPOSE, same reason as RichTextBaseInput: System.Text.Json serializes the DECLARED
// type, so a CONCRETE base made CreateDataSourceRequest.Properties (an
// IDictionary<string, DataSourcePropertyConfigRequest>) emit only the base members -- dropping the
// per-type payload such as "title": {} or "select": { "options": [...] }. Abstract puts it on
// RuntimeTypeConverterFactory's runtime-type-writing path. The UPDATE path already had equivalent
// handling via UpdatePropertyConfigurationRequestConverter; only the CREATE path was missing it.
public abstract class DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
