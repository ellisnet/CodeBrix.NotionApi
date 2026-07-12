using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IRevokeTokenBodyParameters
{
    /// <summary>
    /// The token to be revoked.
    /// </summary>
    [JsonPropertyName("token")]
    string Token { get; set; }
}
