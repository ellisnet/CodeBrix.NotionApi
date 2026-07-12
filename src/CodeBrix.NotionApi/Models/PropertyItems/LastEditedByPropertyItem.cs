using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedByPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "last_edited_by";

    [JsonPropertyName("last_edited_by")]
    public User LastEditedBy { get; set; }
}
