using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedTimeDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.CreatedTime;

    [JsonPropertyName("created_time")]
    public Dictionary<string, object> CreatedTime { get; set; }
}
