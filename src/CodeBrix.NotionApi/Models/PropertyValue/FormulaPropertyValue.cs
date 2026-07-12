using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Formula property value object.
/// </summary>
public class FormulaPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Formula;

    /// <summary>
    ///     A formula described in the database's properties.
    /// </summary>
    [JsonPropertyName("formula")]
    public FormulaValue Formula { get; set; }
}

/// <summary>
///     Formula value object.
/// </summary>
public class FormulaValue
{
    /// <summary>
    ///     Formula value type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     String formula value.
    /// </summary>
    [JsonPropertyName("string")]
    public string String { get; set; }

    /// <summary>
    ///     Number formula value.
    /// </summary>
    [JsonPropertyName("number")]
    public double? Number { get; set; }

    /// <summary>
    ///     Boolean formula value.
    /// </summary>
    [JsonPropertyName("boolean")]
    public bool? Boolean { get; set; }

    /// <summary>
    ///     Date formula value
    /// </summary>
    [JsonPropertyName("date")]
    public Date Date { get; set; }
}
