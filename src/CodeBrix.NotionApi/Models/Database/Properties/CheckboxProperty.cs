using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CheckboxProperty : Property
{
    public override PropertyType Type => PropertyType.Checkbox;

    [JsonPropertyName("checkbox")]
    public Dictionary<string, object> Checkbox { get; set; }
}
