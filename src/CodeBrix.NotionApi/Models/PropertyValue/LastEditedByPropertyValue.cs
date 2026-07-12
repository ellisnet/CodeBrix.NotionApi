using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Last edited by property value object.
/// </summary>
public class LastEditedByPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.LastEditedBy;

    /// <summary>
    ///     Describes the user who last updated this page.
    /// </summary>
    [JsonPropertyName("last_edited_by")]
    public User LastEditedBy { get; set; }
}
