using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableRowBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("table_row")]
    public Info TableRow { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.TableRow;

    public class Info
    {
        [JsonPropertyName("cells")]
        public IEnumerable<IEnumerable<RichTextText>> Cells { get; set; }
    }
}
