using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PhoneNumberDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.PhoneNumber;

    [JsonPropertyName("phone_number")]
    public Dictionary<string, object> PhoneNumber { get; set; }
}
