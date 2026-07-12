using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RelationDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Relation;

    [JsonPropertyName("relation")]
    public RelationInfo Relation { get; set; }
}
