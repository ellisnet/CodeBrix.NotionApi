using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class VideoBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("video")]
    public FileObject Video { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Video;
}
