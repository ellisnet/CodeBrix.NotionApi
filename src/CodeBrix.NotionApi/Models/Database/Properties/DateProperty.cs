using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DateProperty : Property
{
    public override PropertyType Type => PropertyType.Date;

    [JsonPropertyName("date")]
    public Dictionary<string, object> Date { get; set; }
}
