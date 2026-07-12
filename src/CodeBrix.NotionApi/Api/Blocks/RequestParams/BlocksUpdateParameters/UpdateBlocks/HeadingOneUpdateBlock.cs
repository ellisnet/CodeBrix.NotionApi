using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class HeadingOneUpdateBlock : UpdateBlock
{
    [JsonPropertyName("heading_1")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Info Heading_1 { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }
    }
}
