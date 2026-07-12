using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SinglePropertyRelationInfo : RelationInfo
{
    public override string Type => "single_property";

    [JsonPropertyName("single_property")]
    public Dictionary<string, object> SingleProperty { get; set; }
}
