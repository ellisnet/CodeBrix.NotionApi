using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("file")]
    public FileObject File { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.File;
}
