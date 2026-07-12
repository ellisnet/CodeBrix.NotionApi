using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PhoneNumberPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "phone_number";

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}
