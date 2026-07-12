using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedTimeProperty : Property
{
    public override PropertyType Type => PropertyType.LastEditedTime;

    [JsonPropertyName("last_edited_time")]
    public Dictionary<string, object> LastEditedTime { get; set; }
}
