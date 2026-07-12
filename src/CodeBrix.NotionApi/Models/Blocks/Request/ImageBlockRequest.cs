using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ImageBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("image")]
    public FileObject Image { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Image;
}
