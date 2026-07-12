using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CustomEmojiPageIconRequest : IPageIconRequest
{
    [JsonPropertyName("type")]
    public string Type => "custom_emoji";

    [JsonPropertyName("custom_emoji")]
    public CustomEmojiRequest CustomEmoji { get; set; }

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
