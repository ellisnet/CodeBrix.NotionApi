using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RelationDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "relation";

    [JsonPropertyName("relation")]
    public IRelationInfoRequest Relation { get; set; }
}
