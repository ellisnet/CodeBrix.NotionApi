using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class AudioUpdateBlock : UpdateBlock
{
    [JsonPropertyName("audio")]
    public IFileObjectInput Audio { get; set; }
}
