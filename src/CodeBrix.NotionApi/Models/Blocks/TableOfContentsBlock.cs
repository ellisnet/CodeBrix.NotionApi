using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableOfContentsBlock : Block, IColumnChildrenBlock, INonColumnBlock
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
