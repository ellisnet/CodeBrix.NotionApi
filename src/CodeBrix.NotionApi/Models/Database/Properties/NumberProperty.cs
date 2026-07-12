using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class NumberProperty : Property
{
    public override PropertyType Type => PropertyType.Number;

    [JsonPropertyName("number")]
    public Number Number { get; set; }
}

public class Number
{
    [JsonPropertyName("format")]
    public string Format { get; set; }
}
