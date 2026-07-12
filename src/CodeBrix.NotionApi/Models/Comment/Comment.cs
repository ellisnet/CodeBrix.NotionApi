using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Comment : IObject
{
    [JsonPropertyName("parent")]
    public IParentOfComment Parent { get; set; }

    [JsonPropertyName("discussion_id")]
    public string DiscussionId { get; set; }

    [JsonPropertyName("rich_text")]
    public IEnumerable<RichTextBase> RichText { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.Comment;
}
