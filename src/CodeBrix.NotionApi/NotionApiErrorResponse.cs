using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

internal class NotionApiErrorResponse
{
    [JsonPropertyName("code")]
    public NotionAPIErrorCode ErrorCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
