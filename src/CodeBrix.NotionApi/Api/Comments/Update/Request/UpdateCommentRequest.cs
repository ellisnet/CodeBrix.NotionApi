using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UpdateCommentRequest
{
    [JsonIgnore]
    public string CommentId { get; set; }

    [JsonPropertyName("rich_text")]
    public IEnumerable<RichTextBase> RichText { get; set; }
}
