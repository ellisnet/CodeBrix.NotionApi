using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CheckboxDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Checkbox;

    [JsonPropertyName("checkbox")]
    public Dictionary<string, object> Checkbox { get; set; }
}
