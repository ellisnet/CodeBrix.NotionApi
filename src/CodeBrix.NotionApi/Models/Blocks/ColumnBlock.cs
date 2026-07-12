using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ColumnBlock : Block
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Column;

    [JsonPropertyName("column")]
    public Info Column { get; set; }

    public class Info
    {
        [JsonPropertyName("children")]
        public IEnumerable<IColumnChildrenBlock> Children { get; set; }
    }
}
