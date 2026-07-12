using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ColumnBlockRequest : BlockObjectRequest
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Column;

    [JsonPropertyName("column")]
    public Info Column { get; set; }

    public class Info
    {
        [JsonPropertyName("children")]
        public IEnumerable<IColumnChildrenBlockRequest> Children { get; set; }
    }
}
