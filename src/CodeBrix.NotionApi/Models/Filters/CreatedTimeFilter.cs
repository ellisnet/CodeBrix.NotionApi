using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedTimeFilter : SinglePropertyFilter
{
    public CreatedTimeFilter(
        string propertyName,
        DateFilter.Condition createdTime)
    {
        Property = propertyName;
        CreatedTime = createdTime;
    }

    /// <summary>
    /// Gets or sets the created time condition.
    /// </summary>
    [JsonPropertyName("created_time")]
    public DateFilter.Condition CreatedTime { get; set; }
}
