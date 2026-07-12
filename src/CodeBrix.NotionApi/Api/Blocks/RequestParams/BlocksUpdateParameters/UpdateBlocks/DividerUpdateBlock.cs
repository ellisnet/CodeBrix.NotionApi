using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DividerUpdateBlock : IUpdateBlock
{
    public DividerUpdateBlock()
    {
        Divider = new Info();
    }

    [JsonPropertyName("divider")]
    public Info Divider { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    public class Info
    {
    }
}
