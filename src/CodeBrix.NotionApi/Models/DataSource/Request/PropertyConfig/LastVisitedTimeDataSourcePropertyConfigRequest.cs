using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastVisitedTimeDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "last_visited_time";

    [JsonPropertyName("last_visited_time")]
    public IDictionary<string, object> LastVisitedTime { get; set; }
}
