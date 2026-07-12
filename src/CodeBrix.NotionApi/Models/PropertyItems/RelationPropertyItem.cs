using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RelationPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "relation";

    [JsonPropertyName("relation")]
    public ObjectId Relation { get; set; }
}
