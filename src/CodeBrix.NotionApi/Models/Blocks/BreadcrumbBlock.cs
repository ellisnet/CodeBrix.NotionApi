using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BreadcrumbBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("breadcrumb")]
    public Data Breadcrumb { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Breadcrumb;

    public class Data
    {
    }
}
