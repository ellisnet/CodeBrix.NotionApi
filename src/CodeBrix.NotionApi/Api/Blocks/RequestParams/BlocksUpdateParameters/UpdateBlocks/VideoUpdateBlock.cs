using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class VideoUpdateBlock : UpdateBlock
{
    [JsonPropertyName("video")]
    public IFileObjectInput Video { get; set; }
}
