using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextEquationInput : RichTextBaseInput
{
    public override RichTextType Type => RichTextType.Equation;

    [JsonPropertyName("equation")]
    public Equation Equation { get; set; }
}
