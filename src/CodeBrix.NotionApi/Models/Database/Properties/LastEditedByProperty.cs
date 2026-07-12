using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedByProperty : Property
{
    public override PropertyType Type => PropertyType.LastEditedBy;

    [JsonPropertyName("last_edited_by")]
    public Dictionary<string, object> LastEditedBy { get; set; }
}
