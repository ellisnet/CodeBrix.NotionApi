using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RevokeTokenRequest : IRevokeTokenBodyParameters, IBasicAuthenticationParameters
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}
