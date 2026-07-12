using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("file")]
    public FileObject File { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.File;
}
