using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ChildDatabaseBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
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
