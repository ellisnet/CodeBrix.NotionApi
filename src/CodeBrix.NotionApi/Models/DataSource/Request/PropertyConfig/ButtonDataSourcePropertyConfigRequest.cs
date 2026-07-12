using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ButtonDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "button";

    [JsonPropertyName("button")]
    public IDictionary<string, object> Button { get; set; }
}
