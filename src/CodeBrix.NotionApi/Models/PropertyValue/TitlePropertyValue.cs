using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Title property value object.
/// </summary>
public class TitlePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Title;

    /// <summary>
    ///     An array of rich text objects
    /// </summary>
    [JsonPropertyName("title")]
    public List<RichTextBase> Title { get; set; }
}
