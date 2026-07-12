using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ImageBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("image")]
    public FileObject Image { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Image;
}
