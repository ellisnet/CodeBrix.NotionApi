using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Last edited time property value object.
/// </summary>
public class LastEditedTimePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.LastEditedTime;

    /// <summary>
    ///     The date and time when this page was last updated.
    /// </summary>
    [JsonPropertyName("last_edited_time")]
    public string LastEditedTime { get; set; }
}
