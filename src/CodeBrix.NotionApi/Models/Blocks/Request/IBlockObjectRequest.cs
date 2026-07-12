using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IBlockObjectRequest : IObject, IObjectModificationData
{
    [JsonPropertyName("type")]
    BlockType Type { get; }

    [JsonPropertyName("has_children")]
    bool HasChildren { get; set; }

    [JsonPropertyName("parent")]
    IParentOfBlock Parent { get; set; }
}
