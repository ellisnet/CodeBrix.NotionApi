using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class IntrospectTokenResponse
{
    [JsonPropertyName("active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("iat")]
    public long Iat { get; set; }
}
