using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CodeUpdateBlock : UpdateBlock
{
    [JsonPropertyName("code")]
    public Info Code { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("caption")]
        public IEnumerable<RichTextBaseInput> Caption { get; set; }
    }
}
