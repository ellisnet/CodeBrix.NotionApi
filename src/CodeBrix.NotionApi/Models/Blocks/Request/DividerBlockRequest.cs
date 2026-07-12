using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DividerBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("divider")]
    public Data Divider { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Divider;

    public class Data
    {
    }
}
