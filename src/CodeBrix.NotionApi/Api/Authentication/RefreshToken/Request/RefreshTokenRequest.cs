using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RefreshTokenRequest : IRefreshTokenBodyParameters, IBasicAuthenticationParameters
{
    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = "refresh_token";

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}
