using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListEmojisResponse : PaginatedList<CustomEmoji>
{
    [JsonPropertyName("custom_emoji")]
    public Dictionary<string, object> CustomEmoji { get; set; }
}
