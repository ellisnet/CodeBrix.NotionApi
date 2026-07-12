using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class AudioBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("audio")]
    public FileObject Audio { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Audio;
}
