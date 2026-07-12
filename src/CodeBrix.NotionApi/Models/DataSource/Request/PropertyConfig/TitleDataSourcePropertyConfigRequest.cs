using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TitleDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "title";

    [JsonPropertyName("title")]
    public IDictionary<string, object> Title { get; set; }
}
