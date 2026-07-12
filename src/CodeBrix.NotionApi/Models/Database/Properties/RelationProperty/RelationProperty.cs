using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RelationProperty : Property
{
    public override PropertyType Type => PropertyType.Relation;

    [JsonPropertyName("relation")]
    public RelationData Relation { get; set; }
}
