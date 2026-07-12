using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ChildDatabaseBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("child_database")]
    public Info ChildDatabase { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.ChildDatabase;

    public class Info
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
