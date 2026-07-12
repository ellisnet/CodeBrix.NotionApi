using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileUpdateBlock : UpdateBlock
{
    [JsonPropertyName("file")]
    public IFileObjectInput File { get; set; }
}
