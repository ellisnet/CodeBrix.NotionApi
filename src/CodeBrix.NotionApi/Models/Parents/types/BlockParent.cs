using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BlockParent : IParentOfDatabase, IParentOfBlock, IParentOfPage, IParentOfComment
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = ParentTypes.Block;

    [JsonPropertyName(ParentTypes.Block)]
    public string BlockId { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
