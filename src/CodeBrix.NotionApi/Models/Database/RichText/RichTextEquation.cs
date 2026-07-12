using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextEquation : RichTextBase
{
    public override RichTextType Type => RichTextType.Equation;

    [JsonPropertyName("equation")]
    public Equation Equation { get; set; }
}

public class Equation
{
    [JsonPropertyName("expression")]
    public string Expression { get; set; }
}
