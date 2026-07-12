using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Unique Id property value object.
/// </summary>
public class UniqueIdPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.UniqueId;

    /// <summary>
    ///     Unique Id property of database item
    /// </summary>
    [JsonPropertyName("unique_id")]
    public UniqueIdValue UniqueId { get; set; }
}

public class UniqueIdValue
{
    [JsonPropertyName("prefix")]
    public string Prefix { get; set; }

    [JsonPropertyName("number")]
    public double? Number { get; set; }
}
