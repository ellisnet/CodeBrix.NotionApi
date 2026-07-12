using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Select property value object.
/// </summary>
public class SelectPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Select;

    [JsonPropertyName("select")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public SelectOption Select { get; set; }
}
