using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableOfContentsBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("table_of_contents")]
    public Data TableOfContents { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.TableOfContents;

    public class Data
    {
        [JsonPropertyName("color")]
        public Color? Color { get; set; }
    }
}
