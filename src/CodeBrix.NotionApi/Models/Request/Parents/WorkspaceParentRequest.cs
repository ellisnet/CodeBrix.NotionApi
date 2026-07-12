using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class WorkspaceParentRequest : IParentOfPageRequest
{
    [JsonPropertyName("type")]
    public string Type => "workspace";

    [JsonPropertyName("workspace")]
    public bool Workspace => true;

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
