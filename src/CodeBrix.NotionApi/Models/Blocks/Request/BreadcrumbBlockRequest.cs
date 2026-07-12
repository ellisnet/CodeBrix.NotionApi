using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BreadcrumbBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("breadcrumb")]
    public Data Breadcrumb { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Breadcrumb;

    public class Data
    {
    }
}
