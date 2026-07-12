using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedByPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "created_by";

    [JsonPropertyName("created_by")]
    public User CreatedBy { get; set; }
}
