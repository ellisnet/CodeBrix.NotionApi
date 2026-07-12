using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     File property value object.
/// </summary>
public class FilesPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Files;

    /// <summary>
    ///     Array of File Object with name.
    /// </summary>
    [JsonPropertyName("files")]
    public List<FileObjectWithName> Files { get; set; }
}
