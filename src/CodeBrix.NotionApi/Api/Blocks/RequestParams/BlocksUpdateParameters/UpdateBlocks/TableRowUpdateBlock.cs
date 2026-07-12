using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableRowUpdateBlock : UpdateBlock
{
    [JsonPropertyName("table_row")]
    public Info TableRow { get; set; }

    public class Info
    {
        [JsonPropertyName("cells")]
        public IEnumerable<IEnumerable<RichTextTextInput>> Cells { get; set; }
    }
}
