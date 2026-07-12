using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DividerBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("divider")]
    public Data Divider { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Divider;

    public class Data
    {
    }
}
