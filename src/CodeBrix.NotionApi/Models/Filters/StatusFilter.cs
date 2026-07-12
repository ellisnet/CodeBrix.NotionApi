using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class StatusFilter : SinglePropertyFilter, IRollupSubPropertyFilter
{
    public StatusFilter(
        string propertyName,
        string equal = null,
        string doesNotEqual = null,
        bool? isEmpty = null,
        bool? isNotEmpty = null)
    {
        Property = propertyName;

        Status = new Condition(
            equal,
            doesNotEqual,
            isEmpty,
            isNotEmpty
        );
    }

    [JsonPropertyName("status")]
    public Condition Status { get; set; }

    public class Condition
    {
        public Condition(
            string equal = null,
            string doesNotEqual = null,
            bool? isEmpty = null,
            bool? isNotEmpty = null)
        {
            Equal = equal;
            DoesNotEqual = doesNotEqual;
            IsEmpty = isEmpty;
            IsNotEmpty = isNotEmpty;
        }

        [JsonPropertyName("equals")]
        public string Equal { get; set; }

        [JsonPropertyName("does_not_equal")]
        public string DoesNotEqual { get; set; }

        [JsonPropertyName("is_empty")]
        public bool? IsEmpty { get; set; }

        [JsonPropertyName("is_not_empty")]
        public bool? IsNotEmpty { get; set; }
    }
}
