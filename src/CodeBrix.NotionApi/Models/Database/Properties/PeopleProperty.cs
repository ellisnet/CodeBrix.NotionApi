using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PeopleProperty : Property
{
    public override PropertyType Type => PropertyType.People;

    [JsonPropertyName("people")]
    public Dictionary<string, object> People { get; set; }
}
