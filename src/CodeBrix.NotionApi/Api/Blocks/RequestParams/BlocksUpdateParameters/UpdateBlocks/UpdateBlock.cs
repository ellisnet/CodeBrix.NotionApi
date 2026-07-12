using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public abstract class UpdateBlock : IUpdateBlock
{
    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }
}
