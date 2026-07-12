using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedTimeFilter : SinglePropertyFilter
{
    public LastEditedTimeFilter(
        string propertyName,
        DateFilter.Condition lastEditedTime)
    {
        Property = propertyName;
        LastEditedTime = lastEditedTime;
    }

    /// <summary>
    /// Gets or sets the last edited time condition.
    /// </summary>
    [JsonPropertyName("last_edited_time")]
    public DateFilter.Condition LastEditedTime { get; set; }
}
