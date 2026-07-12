using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class WorkspaceIntegrationOwner : IBotOwner
{
    [JsonPropertyName("workspace")]
    public bool Workspace { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
