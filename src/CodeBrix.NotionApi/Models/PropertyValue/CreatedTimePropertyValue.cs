using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Created time property value object.
/// </summary>
public class CreatedTimePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.CreatedTime;

    /// <summary>
    ///     The date and time when this page was created.
    /// </summary>
    [JsonPropertyName("created_time")]
    public string CreatedTime { get; set; }
}
