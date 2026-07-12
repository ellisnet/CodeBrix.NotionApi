using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class NumberDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.Number;

    [JsonPropertyName("number")]
    public NumberResponse Number { get; set; }
}

public class NumberResponse
{
    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
