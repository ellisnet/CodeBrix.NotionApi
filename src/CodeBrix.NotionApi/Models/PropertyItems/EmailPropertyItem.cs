using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmailPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "email";

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
