using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Rich Text property value object.
/// </summary>
public class RichTextPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.RichText;

    /// <summary>
    ///     List of rich text objects
    /// </summary>
    [JsonPropertyName("rich_text")]
    public List<RichTextBase> RichText { get; set; }
}
