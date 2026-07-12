using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ChildPageBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("child_page")]
    public Info ChildPage { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.ChildPage;

    public class Info
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
