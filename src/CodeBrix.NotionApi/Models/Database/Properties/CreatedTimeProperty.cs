using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedTimeProperty : Property
{
    public override PropertyType Type => PropertyType.CreatedTime;

    [JsonPropertyName("created_time")]
    public Dictionary<string, object> CreatedTime { get; set; }
}
