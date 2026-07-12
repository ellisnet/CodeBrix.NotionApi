using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IPageCoverRequest
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
