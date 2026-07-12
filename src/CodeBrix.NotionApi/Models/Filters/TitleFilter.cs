using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TitleFilter : SinglePropertyFilter
{
    public TitleFilter(
        string propertyName,
        string equal = null,
        string doesNotEqual = null,
        string contains = null,
        string doesNotContain = null,
        string startsWith = null,
        string endsWith = null,
        bool? isEmpty = null,
        bool? isNotEmpty = null)
    {
        Property = propertyName;

        Title = new TextFilter.Condition(
            equal,
            doesNotEqual,
            contains,
            doesNotContain,
            startsWith,
            endsWith,
            isEmpty,
            isNotEmpty
        );
    }

    [JsonPropertyName("title")]
    public TextFilter.Condition Title { get; set; }
}
