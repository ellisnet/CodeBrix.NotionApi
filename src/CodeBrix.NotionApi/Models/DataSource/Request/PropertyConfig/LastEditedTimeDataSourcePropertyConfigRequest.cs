using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedTimeDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "last_edited_time";

    [JsonPropertyName("last_edited_time")]
    public IDictionary<string, object> LastEditedTime { get; set; }
}
