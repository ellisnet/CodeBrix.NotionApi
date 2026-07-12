using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UserOwner : IBotOwner
{
    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
