using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IRefreshTokenBodyParameters
{
    /// <summary>
    /// A constant string: "refresh_token"
    /// </summary>
    [JsonPropertyName("grant_type")]
    string GrantType { get; set; }

    /// <summary>
    /// A unique token that Notion generates to refresh your token, generated when a user initiates the OAuth flow.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    string RefreshToken { get; set; }
}
