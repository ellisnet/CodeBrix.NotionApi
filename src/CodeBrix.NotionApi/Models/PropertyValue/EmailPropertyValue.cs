using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Email property value object.
/// </summary>
public class EmailPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Email;

    /// <summary>
    ///     Describes an email address.
    /// </summary>
    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Email { get; set; }
}
