using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ButtonDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Button;

    [JsonPropertyName("button")]
    public Dictionary<string, object> Button { get; set; }
}
