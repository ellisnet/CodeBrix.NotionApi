using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class NumberPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "number";

    [JsonPropertyName("number")]
    public double? Number { get; set; }
}
