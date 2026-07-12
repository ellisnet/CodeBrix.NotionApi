using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class WorkspaceParent : IParentOfDatabase, IParentOfBlock, IParentOfPage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = ParentTypes.Workspace;

    [JsonPropertyName(ParentTypes.Workspace)]
    public bool Workspace { get; set; } = true;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
