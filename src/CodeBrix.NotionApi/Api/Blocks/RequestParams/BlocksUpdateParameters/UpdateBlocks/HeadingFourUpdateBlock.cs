using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingFourUpdateBlock : UpdateBlock
{
    [JsonPropertyName("heading_4")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_4 { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }
    }
}
