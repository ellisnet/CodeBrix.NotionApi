using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("table")]
    public Info Table { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Table;

    public class Info
    {
        [JsonPropertyName("table_width")]
        public int TableWidth { get; set; }

        [JsonPropertyName("has_column_header")]
        public bool HasColumnHeader { get; set; }

        [JsonPropertyName("has_row_header")]
        public bool HasRowHeader { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<TableRowBlock> Children { get; set; }
    }
}
