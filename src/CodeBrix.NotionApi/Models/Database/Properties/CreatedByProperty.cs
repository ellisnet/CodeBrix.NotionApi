using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedByProperty : Property
{
    public override PropertyType Type => PropertyType.CreatedBy;

    [JsonPropertyName("created_by")]
    public Dictionary<string, object> CreatedBy { get; set; }
}
