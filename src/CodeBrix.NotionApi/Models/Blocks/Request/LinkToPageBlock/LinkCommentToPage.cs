using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LinkCommentToPage : ILinkToPage
{
    [JsonPropertyName("type")]
    public string Type => "comment_id";

    [JsonPropertyName("comment_id")]
    public string CommentId { get; set; }
}
