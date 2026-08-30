using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DeletedViewQueryResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}
