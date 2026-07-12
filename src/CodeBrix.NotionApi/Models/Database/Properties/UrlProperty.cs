using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UrlProperty : Property
{
    public override PropertyType Type => PropertyType.Url;

    [JsonPropertyName("url")]
    public Dictionary<string, object> Url { get; set; }
}
