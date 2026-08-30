using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreateCommentRequest : ICreateDiscussionCommentBodyParameters, ICreatePageCommentBodyParameters
{
    // System.Text.Json does not honour [JsonPropertyName] declared on an INTERFACE member, so the
    // names from ICreateDiscussionCommentBodyParameters / ICreatePageCommentBodyParameters have to
    // be repeated here. Without them the body went out as "discussionId" / "richText" and Notion
    // answered "body.rich_text should be defined, instead was `undefined`".
    [JsonPropertyName("discussion_id")]
    public string DiscussionId { get; set; }

    [JsonPropertyName("rich_text")]
    public IEnumerable<RichTextBaseInput> RichText { get; set; }

    [JsonPropertyName("parent")]
    public ParentPageInput Parent { get; set; }

    public static CreateCommentRequest CreatePageComment(
        ParentPageInput parent,
        IEnumerable<RichTextBaseInput> richText)
    {
        return new CreateCommentRequest
        {
            Parent = parent,
            RichText = richText
        };
    }

    public static CreateCommentRequest CreateDiscussionComment(
        string discussionId,
        IEnumerable<RichTextBaseInput> richText)
    {
        return new CreateCommentRequest
        {
            DiscussionId = discussionId,
            RichText = richText
        };
    }
}
