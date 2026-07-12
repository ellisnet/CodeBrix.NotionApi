using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreateTokenRequest : ICreateTokenBodyParameters, IBasicAuthenticationParameters
{
    [JsonPropertyName("grant_type")]
    public string GrantType => "authorization_code";

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("redirect_uri")]
    public string RedirectUri { get; set; }

    [JsonPropertyName("external_account")]
    public ExternalAccount ExternalAccount { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}
