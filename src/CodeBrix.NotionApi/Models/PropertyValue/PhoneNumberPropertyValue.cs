using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Phone number property value object.
/// </summary>
public class PhoneNumberPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.PhoneNumber;

    /// <summary>
    ///     Phone number value
    /// </summary>
    [JsonPropertyName("phone_number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string PhoneNumber { get; set; }
}
