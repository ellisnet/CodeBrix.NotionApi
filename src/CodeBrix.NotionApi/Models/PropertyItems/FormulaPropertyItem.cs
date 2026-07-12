using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FormulaPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "formula";

    [JsonPropertyName("formula")]
    public FormulaValue Formula { get; set; }
}
