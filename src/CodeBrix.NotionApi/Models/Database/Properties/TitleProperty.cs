using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TitleProperty : Property
{
    public override PropertyType Type => PropertyType.Title;

    [JsonPropertyName("title")]
    public Dictionary<string, object> Title { get; set; }
}
