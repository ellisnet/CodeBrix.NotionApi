using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class StatusDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Status;

    [JsonPropertyName("status")]
    public StatusConfig Status { get; set; }
}

public class StatusConfig
{
    [JsonPropertyName("options")]
    public IEnumerable<StatusOption> Options { get; set; }

    [JsonPropertyName("groups")]
    public IEnumerable<StatusGroup> Groups { get; set; }

    /// <summary>
    /// Additional data for future compatibility.
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class StatusOption
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }
}

public class StatusGroup
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }

    [JsonPropertyName("option_ids")]
    public IEnumerable<string> OptionIds { get; set; }
}
