using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IParentOfDatabaseRequest
{
    [JsonPropertyName("type")]
    string Type { get; }
}
