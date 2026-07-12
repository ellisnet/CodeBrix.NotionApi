using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PhoneNumberDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "phone_number";

    [JsonPropertyName("phone_number")]
    public IDictionary<string, object> PhoneNumber { get; set; }
}
