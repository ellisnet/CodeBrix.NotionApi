using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Checkbox property value objects contain a boolean within the checkbox property.
/// </summary>
public class CheckboxPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Checkbox;

    [JsonPropertyName("checkbox")]
    public bool Checkbox { get; set; }
}
