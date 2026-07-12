using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PageParent : IParentOfDatabase, IParentOfBlock, IParentOfPage, IParentOfComment
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = ParentTypes.Page;

    [JsonPropertyName(ParentTypes.Page)]
    public string PageId { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
