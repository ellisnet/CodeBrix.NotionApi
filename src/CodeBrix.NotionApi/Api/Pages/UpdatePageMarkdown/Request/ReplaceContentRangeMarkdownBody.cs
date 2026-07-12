using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Replaces a range of existing page content with new markdown.
/// </summary>
public class ReplaceContentRangeMarkdownBody : UpdatePageMarkdownBody
{
    public override string Type => "replace_content_range";

    [JsonPropertyName("replace_content_range")]
    public ReplaceContentRangeData ReplaceContentRange { get; set; }
}

public class ReplaceContentRangeData
{
    /// <summary>
    /// The new enhanced markdown content to replace the matched range.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Ellipsis-format selection of content to replace (e.g. "start text...end text").
    /// </summary>
    [JsonPropertyName("content_range")]
    public string ContentRange { get; set; }

    /// <summary>
    /// Set to <c>true</c> to allow the operation to delete child pages or databases.
    /// Defaults to <c>false</c>.
    /// </summary>
    [JsonPropertyName("allow_deleting_content")]
    public bool? AllowDeletingContent { get; set; }
}
