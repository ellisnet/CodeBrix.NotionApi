using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UniqueIdFilter : SinglePropertyFilter
{
    public UniqueIdFilter(
        string propertyName,
        NumberFilter.Condition uniqueId)
    {
        Property = propertyName;
        UniqueId = uniqueId;
    }

    /// <summary>
    /// Gets or sets the unique id condition.
    /// </summary>
    [JsonPropertyName("unique_id")]
    public NumberFilter.Condition UniqueId { get; set; }
}
