using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CheckboxFilter : SinglePropertyFilter, IRollupSubPropertyFilter
{
    public CheckboxFilter(
        string propertyName,
        bool? equal = null,
        bool? doesNotEqual = null)
    {
        Property = propertyName;
        Checkbox = new Condition(equal, doesNotEqual);
    }

    [JsonPropertyName("checkbox")]
    public Condition Checkbox { get; set; }

    public class Condition
    {
        public Condition(bool? equal = null, bool? doesNotEqual = null)
        {
            Equal = equal;
            DoesNotEqual = doesNotEqual;
        }

        [JsonPropertyName("equals")]
        public bool? Equal { get; set; }

        [JsonPropertyName("does_not_equal")]
        public bool? DoesNotEqual { get; set; }
    }
}
