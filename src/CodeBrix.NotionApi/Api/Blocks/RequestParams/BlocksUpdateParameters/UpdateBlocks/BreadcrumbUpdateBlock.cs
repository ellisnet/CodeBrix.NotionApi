using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BreadcrumbUpdateBlock : IUpdateBlock
{
    public BreadcrumbUpdateBlock()
    {
        Breadcrumb = new Info();
    }

    [JsonPropertyName("breadcrumb")]
    public Info Breadcrumb { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    public class Info
    {
    }
}
