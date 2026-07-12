using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Person
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
