using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MultiSelectFilter : SinglePropertyFilter, IRollupSubPropertyFilter
{
    public MultiSelectFilter(
        string propertyName,
        string contains = null,
        string doesNotContain = null,
        bool? isEmpty = null,
        bool? isNotEmpty = null)
    {
        Property = propertyName;

        MultiSelect = new Condition(
            contains,
            doesNotContain,
            isEmpty,
            isNotEmpty
        );
    }

    [JsonPropertyName("multi_select")]
    public Condition MultiSelect { get; set; }

    public class Condition
    {
        public Condition(
            string contains = null,
            string doesNotContain = null,
            bool? isEmpty = null,
            bool? isNotEmpty = null)
        {
            Contains = contains;
            DoesNotContain = doesNotContain;
            IsEmpty = isEmpty;
            IsNotEmpty = isNotEmpty;
        }

        [JsonPropertyName("contains")]
        public string Contains { get; set; }

        [JsonPropertyName("does_not_contain")]
        public string DoesNotContain { get; set; }

        [JsonPropertyName("is_empty")]
        public bool? IsEmpty { get; set; }

        [JsonPropertyName("is_not_empty")]
        public bool? IsNotEmpty { get; set; }
    }
}
