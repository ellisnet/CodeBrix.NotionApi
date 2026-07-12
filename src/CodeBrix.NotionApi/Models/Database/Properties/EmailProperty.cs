using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmailProperty : Property
{
    public override PropertyType Type => PropertyType.Email;

    [JsonPropertyName("email")]
    public Dictionary<string, object> Email { get; set; }
}
