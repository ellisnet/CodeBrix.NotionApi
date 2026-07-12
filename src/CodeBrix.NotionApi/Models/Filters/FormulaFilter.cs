using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FormulaFilter : SinglePropertyFilter
{
    public FormulaFilter(
        string propertyName,
        TextFilter.Condition @string = null,
        CheckboxFilter.Condition checkbox = null,
        NumberFilter.Condition number = null,
        DateFilter.Condition date = null)
    {
        Property = propertyName;

        Formula = new Condition(
            @string,
            checkbox,
            number,
            date
        );
    }

    [JsonPropertyName("formula")]
    public Condition Formula { get; set; }

    public class Condition
    {
        public Condition(
            TextFilter.Condition @string = null,
            CheckboxFilter.Condition checkbox = null,
            NumberFilter.Condition number = null,
            DateFilter.Condition date = null)
        {
            String = @string;
            Checkbox = checkbox;
            Number = number;
            Date = date;
        }

        [JsonPropertyName("string")]
        public TextFilter.Condition String { get; set; }

        [JsonPropertyName("checkbox")]
        public CheckboxFilter.Condition Checkbox { get; set; }

        [JsonPropertyName("number")]
        public NumberFilter.Condition Number { get; set; }

        [JsonPropertyName("date")]
        public DateFilter.Condition Date { get; set; }
    }
}
