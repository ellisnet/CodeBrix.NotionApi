using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedTimeDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "created_time";

    [JsonPropertyName("created_time")]
    public IDictionary<string, object> CreatedTime { get; set; }
}
