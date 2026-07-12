using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SelectPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "select";

    [JsonPropertyName("select")]
    public SelectOption Select { get; set; }
}
