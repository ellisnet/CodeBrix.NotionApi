using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextProperty : Property
{
    public override PropertyType Type => PropertyType.RichText;

    [JsonPropertyName("rich_text")]
    public Dictionary<string, object> RichText { get; set; }
}
