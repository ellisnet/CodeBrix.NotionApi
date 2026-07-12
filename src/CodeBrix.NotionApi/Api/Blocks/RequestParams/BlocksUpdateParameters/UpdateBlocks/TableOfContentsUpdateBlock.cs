using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableOfContentsUpdateBlock : IUpdateBlock
{
    public TableOfContentsUpdateBlock()
    {
        TableOfContents = new Info();
    }

    [JsonPropertyName("table_of_contents")]
    public Info TableOfContents { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    public class Info
    {
    }
}
