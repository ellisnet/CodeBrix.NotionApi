using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingTwoUpdateBlock : UpdateBlock
{
    [JsonPropertyName("heading_2")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_2 { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }
    }
}
