using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ToDoUpdateBlock : UpdateBlock
{
    [JsonPropertyName("to_do")]
    public Info ToDo { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }

        [JsonPropertyName("checked")]
        public bool IsChecked { get; set; }
    }
}
