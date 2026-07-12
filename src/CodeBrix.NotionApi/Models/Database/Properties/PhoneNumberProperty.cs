using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PhoneNumberProperty : Property
{
    public override PropertyType Type => PropertyType.PhoneNumber;

    [JsonPropertyName("phone_number")]
    public Dictionary<string, object> PhoneNumber { get; set; }
}
