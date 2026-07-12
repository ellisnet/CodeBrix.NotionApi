using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SinglePropertyRelation : RelationData
{
    public override RelationType Type => RelationType.Single;

    [JsonPropertyName("single_property")]
    public Dictionary<string, object> SingleProperty { get; set; }
}
