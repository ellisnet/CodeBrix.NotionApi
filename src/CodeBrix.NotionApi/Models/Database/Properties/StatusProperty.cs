using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class StatusProperty : Property
{
    public override PropertyType Type => PropertyType.Status;

    [JsonPropertyName("status")]
    public StatusConfig Status { get; set; }
}
