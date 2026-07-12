using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Multi-select property value object.
/// </summary>
public class MultiSelectPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.MultiSelect;

    /// <summary>
    ///     An array of multi-select option values.
    /// </summary>
    [JsonPropertyName("multi_select")]
    public List<SelectOption> MultiSelect { get; set; }
}
