using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FormulaDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Formula;

    [JsonPropertyName("formula")]
    public FormulaResponse Formula { get; set; }
}

public class FormulaResponse
{
    [JsonPropertyName("expression")]
    public string Expression { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
