using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ImageUpdateBlock : UpdateBlock
{
    [JsonPropertyName("image")]
    public IFileObjectInput Image { get; set; }
}
