using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Owner
{
    [JsonPropertyName("workspace")]
    public bool Workspace { get; set; }
}
