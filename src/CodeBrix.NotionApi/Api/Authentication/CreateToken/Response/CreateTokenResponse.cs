using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreateTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "bearer";

    [JsonPropertyName("bot_id")]
    public string BotId { get; set; }

    [JsonPropertyName("duplicated_template_id")]
    public string DuplicatedTemplateId { get; set; }

    [JsonPropertyName("owner")]
    public IBotOwner Owner { get; set; }

    [JsonPropertyName("workspace_icon")]
    public string WorkspaceIcon { get; set; }

    [JsonPropertyName("workspace_id")]
    public string WorkspaceId { get; set; }

    [JsonPropertyName("workspace_name")]
    public string WorkspaceName { get; set; }
}
