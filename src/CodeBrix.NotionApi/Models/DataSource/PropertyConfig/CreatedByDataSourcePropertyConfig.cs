using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedByDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.CreatedBy;

    [JsonPropertyName("created_by")]
    public Dictionary<string, object> CreatedBy { get; set; }
}
