using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingThreeUpdateBlock : UpdateBlock
{
    [JsonPropertyName("heading_3")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_3 { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }
    }
}
