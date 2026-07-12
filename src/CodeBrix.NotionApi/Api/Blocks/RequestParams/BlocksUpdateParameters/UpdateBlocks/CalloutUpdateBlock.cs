using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CalloutUpdateBlock : UpdateBlock
{
    [JsonPropertyName("callout")]
    public Info Callout { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }

        [JsonPropertyName("icon")]
        public IPageIconRequest Icon { get; set; }
    }
}
