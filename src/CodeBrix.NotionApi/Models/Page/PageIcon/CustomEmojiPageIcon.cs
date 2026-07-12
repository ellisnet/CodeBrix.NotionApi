using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CustomEmojiPageIcon : IPageIcon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageIconTypes.CustomEmoji;

    [JsonPropertyName(PageIconTypes.CustomEmoji)]
    public CustomEmoji CustomEmoji { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}

public class IconPageIcon : IPageIcon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageIconTypes.Icon;
    
    [JsonPropertyName(PageIconTypes.Icon)]
    public IconObject Icon { get; set; }
    
    public class IconObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
