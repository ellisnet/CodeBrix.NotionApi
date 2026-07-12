using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmojiPageIcon : IPageIcon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageIconTypes.Emoji;

    [JsonPropertyName(PageIconTypes.Emoji)]
    public string Emoji { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
