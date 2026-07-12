using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedByFilter : SinglePropertyFilter
{
    public LastEditedByFilter(
        string propertyName,
        PeopleFilter.Condition lastEditedBy)
    {
        Property = propertyName;
        LastEditedBy = lastEditedBy;
    }

    /// <summary>
    /// Gets or sets the last edited by condition.
    /// </summary>
    [JsonPropertyName("last_edited_by")]
    public PeopleFilter.Condition LastEditedBy { get; set; }
}
