using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedByFilter : SinglePropertyFilter
{
    public CreatedByFilter(
        string propertyName,
        PeopleFilter.Condition createdBy)
    {
        Property = propertyName;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets or sets the created by condition.
    /// </summary>
    [JsonPropertyName("created_by")]
    public PeopleFilter.Condition CreatedBy { get; set; }
}
