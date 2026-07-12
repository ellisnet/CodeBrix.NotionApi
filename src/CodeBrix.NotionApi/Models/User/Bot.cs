using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Bot
{
    [JsonPropertyName("owner")]
    public IBotOwner Owner { get; set; }
}
