using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UnsupportedBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Unsupported;

    [JsonPropertyName("unsupported")]
    public UnsupportedBlockResponse Unsupported { get; set; }
}

public class UnsupportedBlockResponse
{
    [JsonPropertyName("block_type")]
    public string BlockType { get; set; }
}
