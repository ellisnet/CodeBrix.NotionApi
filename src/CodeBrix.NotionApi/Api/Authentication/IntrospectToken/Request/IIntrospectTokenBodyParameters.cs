using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IIntrospectTokenBodyParameters
{
    /// <summary>
    /// The access token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; }
}
