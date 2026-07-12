using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ColumnListBlock : Block, INonColumnBlock
{
    [JsonPropertyName("column_list")]
    public Info ColumnList { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.ColumnList;

    public class Info
    {
        [JsonPropertyName("children")]
        public IEnumerable<ColumnBlock> Children { get; set; }
    }
}
