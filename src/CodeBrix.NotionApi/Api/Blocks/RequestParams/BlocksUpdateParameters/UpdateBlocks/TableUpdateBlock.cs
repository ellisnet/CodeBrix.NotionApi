using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TableUpdateBlock : UpdateBlock
{
    [JsonPropertyName("table")]
    public Info Table { get; set; }

    public class Info
    {
        [JsonPropertyName("has_column_header")]
        public bool HasColumnHeader { get; set; }

        [JsonPropertyName("has_row_header")]
        public bool HasRowHeader { get; set; }
    }
}
