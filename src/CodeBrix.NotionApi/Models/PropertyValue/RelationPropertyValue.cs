using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Relation property value object.
/// </summary>
public class RelationPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Relation;

    /// <summary>
    ///     Array of page references
    /// </summary>
    [JsonPropertyName("relation")]
    public List<ObjectId> Relation { get; set; }
}
