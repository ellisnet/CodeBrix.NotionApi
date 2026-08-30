using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DateFilter : SinglePropertyFilter, IRollupSubPropertyFilter
{
    public DateFilter(
        string propertyName,
        RelativeDateValue? equal = null,
        RelativeDateValue? before = null,
        RelativeDateValue? after = null,
        RelativeDateValue? onOrBefore = null,
        RelativeDateValue? onOrAfter = null,
        Dictionary<string, object> pastWeek = null,
        Dictionary<string, object> pastMonth = null,
        Dictionary<string, object> pastYear = null,
        Dictionary<string, object> nextWeek = null,
        Dictionary<string, object> nextMonth = null,
        Dictionary<string, object> nextYear = null,
        bool? isEmpty = null,
        bool? isNotEmpty = null)
    {
        Property = propertyName;

        Date = new Condition(
            equal,
            before,
            after,
            onOrBefore,
            onOrAfter,
            pastWeek,
            pastMonth,
            pastYear,
            nextWeek,
            nextMonth,
            nextYear,
            isEmpty,
            isNotEmpty
        );
    }

    [JsonPropertyName("date")]
    public Condition Date { get; set; }

    public class Condition
    {
        public Condition(
            RelativeDateValue? equal = null,
            RelativeDateValue? before = null,
            RelativeDateValue? after = null,
            RelativeDateValue? onOrBefore = null,
            RelativeDateValue? onOrAfter = null,
            Dictionary<string, object> pastWeek = null,
            Dictionary<string, object> pastMonth = null,
            Dictionary<string, object> pastYear = null,
            Dictionary<string, object> nextWeek = null,
            Dictionary<string, object> nextMonth = null,
            Dictionary<string, object> nextYear = null,
            bool? isEmpty = null,
            bool? isNotEmpty = null)
        {
            Equal = equal;
            Before = before;
            After = after;
            OnOrBefore = onOrBefore;
            OnOrAfter = onOrAfter;
            PastWeek = pastWeek;
            PastMonth = pastMonth;
            PastYear = pastYear;
            NextWeek = nextWeek;
            NextMonth = nextMonth;
            NextYear = nextYear;
            IsEmpty = isEmpty;
            IsNotEmpty = isNotEmpty;
        }

        [JsonPropertyName("equals")]
        public RelativeDateValue? Equal { get; set; }

        [JsonPropertyName("before")]
        public RelativeDateValue? Before { get; set; }

        [JsonPropertyName("after")]
        public RelativeDateValue? After { get; set; }

        [JsonPropertyName("on_or_before")]
        public RelativeDateValue? OnOrBefore { get; set; }

        [JsonPropertyName("on_or_after")]
        public RelativeDateValue? OnOrAfter { get; set; }

        [JsonPropertyName("past_week")]
        public Dictionary<string, object> PastWeek { get; set; }

        [JsonPropertyName("past_month")]
        public Dictionary<string, object> PastMonth { get; set; }

        [JsonPropertyName("past_year")]
        public Dictionary<string, object> PastYear { get; set; }

        [JsonPropertyName("next_week")]
        public Dictionary<string, object> NextWeek { get; set; }

        [JsonPropertyName("next_month")]
        public Dictionary<string, object> NextMonth { get; set; }

        [JsonPropertyName("next_year")]
        public Dictionary<string, object> NextYear { get; set; }

        [JsonPropertyName("is_empty")]
        public bool? IsEmpty { get; set; }

        [JsonPropertyName("is_not_empty")]
        public bool? IsNotEmpty { get; set; }
    }
}
