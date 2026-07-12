using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ColumnListBlockRequest : BlockObjectRequest, INonColumnBlockRequest
{
    [JsonPropertyName("column_list")]
    public Info ColumnList { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.ColumnList;

    public class Info
    {
        [JsonPropertyName("children")]
        public IEnumerable<ColumnBlockRequest> Children { get; set; }
    }
}
