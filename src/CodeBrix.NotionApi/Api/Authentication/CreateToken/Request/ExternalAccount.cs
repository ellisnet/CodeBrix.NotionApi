using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// External account info
/// </summary>
public class ExternalAccount
{
    /// <summary>
    /// External account key
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }

    /// <summary>
    /// External account name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
