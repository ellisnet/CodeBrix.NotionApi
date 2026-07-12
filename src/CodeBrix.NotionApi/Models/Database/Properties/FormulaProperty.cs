using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FormulaProperty : Property
{
    public override PropertyType Type => PropertyType.Formula;

    [JsonPropertyName("formula")]
    public Formula Formula { get; set; }
}

public class Formula
{
    [JsonPropertyName("expression")]
    public string Expression { get; set; }
}
