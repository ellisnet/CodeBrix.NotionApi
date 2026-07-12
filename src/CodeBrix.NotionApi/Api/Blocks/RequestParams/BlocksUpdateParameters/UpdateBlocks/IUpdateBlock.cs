using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IUpdateBlock
{
    [JsonPropertyName("in_trash")]
    bool InTrash { get; set; }
}
