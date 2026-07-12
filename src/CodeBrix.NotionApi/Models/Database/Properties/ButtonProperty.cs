using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ButtonProperty : Property
{
    public override PropertyType Type => PropertyType.Button;

    [JsonPropertyName("button")]
    public Dictionary<string,object> Button { get; set; }
}
