using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Number formula property value object.
/// </summary>
public class NumberPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Number;

    /// <summary>
    ///     Value of number
    /// </summary>
    [JsonPropertyName("number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public double? Number { get; set; }
}
