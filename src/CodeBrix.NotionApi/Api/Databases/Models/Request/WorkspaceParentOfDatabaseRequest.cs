using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class WorkspaceParentOfDatabaseRequest : IParentOfDatabaseRequest
{
    [JsonPropertyName("type")]
    public string Type => "workspace";

    [JsonPropertyName("workspace")]
    public bool Workspace => true;
}
