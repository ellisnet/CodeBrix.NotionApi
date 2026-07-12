using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CheckboxPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "checkbox";

    [JsonPropertyName("checkbox")]
    public bool Checkbox { get; set; }
}
