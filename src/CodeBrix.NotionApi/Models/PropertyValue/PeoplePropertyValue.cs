using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     People property value object.
/// </summary>
public class PeoplePropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.People;

    /// <summary>
    ///     List of users.
    /// </summary>
    [JsonPropertyName("people")]
    public List<User> People { get; set; }
}
