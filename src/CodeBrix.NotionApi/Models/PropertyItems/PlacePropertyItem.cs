using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PlacePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "place";

    [JsonPropertyName("place")]
    public PlacePropertyValue.PlaceInfo Place { get; set; }
}
