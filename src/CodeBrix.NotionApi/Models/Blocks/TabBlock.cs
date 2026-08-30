using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TabBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Tab;

    [JsonPropertyName("tab")]
    public Data Tab { get; set; }

    public class Data
    {
    }
}
