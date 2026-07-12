using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class VideoBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("video")]
    public FileObject Video { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Video;
}
