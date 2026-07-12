using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PeoplePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "people";

    [JsonPropertyName("people")]
    public User People { get; set; }
}
