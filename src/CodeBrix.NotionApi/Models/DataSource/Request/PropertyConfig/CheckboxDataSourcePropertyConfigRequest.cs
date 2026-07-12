using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CheckboxDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "checkbox";

    [JsonPropertyName("checkbox")]
    public IDictionary<string, object> Checkbox { get; set; }
}
