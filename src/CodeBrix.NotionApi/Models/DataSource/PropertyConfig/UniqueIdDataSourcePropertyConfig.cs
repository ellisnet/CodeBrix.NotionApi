using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UniqueIdDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.UniqueId;

    [JsonPropertyName("unique_id")]
    public Dictionary<string, object> UniqueId { get; set; }
}
