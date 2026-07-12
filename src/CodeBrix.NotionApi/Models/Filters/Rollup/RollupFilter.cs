using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RollupFilter : SinglePropertyFilter
{
    public RollupFilter(
        string propertyName,
        IRollupSubPropertyFilter any = null,
        IRollupSubPropertyFilter none = null,
        IRollupSubPropertyFilter every = null,
        DateFilter.Condition date = null,
        NumberFilter.Condition number = null)
    {
        Property = propertyName;

        Rollup = new Condition(
            any,
            none,
            every,
            date,
            number
        );
    }

    [JsonPropertyName("rollup")]
    public Condition Rollup { get; set; }

    public class Condition
    {
        public Condition(
            IRollupSubPropertyFilter any = null,
            IRollupSubPropertyFilter none = null,
            IRollupSubPropertyFilter every = null,
            DateFilter.Condition date = null,
            NumberFilter.Condition number = null)
        {
            Any = any;
            None = none;
            Every = every;
            Date = date;
            Number = number;
        }

        [JsonPropertyName("any")]
        public IRollupSubPropertyFilter Any { get; set; }

        [JsonPropertyName("none")]
        public IRollupSubPropertyFilter None { get; set; }

        [JsonPropertyName("every")]
        public IRollupSubPropertyFilter Every { get; set; }

        [JsonPropertyName("date")]
        public DateFilter.Condition Date { get; set; }

        [JsonPropertyName("number")]
        public NumberFilter.Condition Number { get; set; }
    }
}
