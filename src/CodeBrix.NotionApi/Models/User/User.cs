using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class User : IObject
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("person")]
    public Person Person { get; set; }

    [JsonPropertyName("bot")]
    public Bot Bot { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.User;

    [JsonPropertyName("id")]
    public string Id { get; set; }
}
