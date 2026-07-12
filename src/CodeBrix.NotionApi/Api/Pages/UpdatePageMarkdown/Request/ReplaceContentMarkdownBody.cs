using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Replaces the entire page content with new markdown.
/// </summary>
public class ReplaceContentMarkdownBody : UpdatePageMarkdownBody
{
    public override string Type => "replace_content";

    [JsonPropertyName("replace_content")]
    public ReplaceContentData ReplaceContent { get; set; }
}

public class ReplaceContentData
{
    /// <summary>
    /// The new enhanced markdown content to replace the entire page content.
    /// </summary>
    [JsonPropertyName("new_str")]
    public string NewStr { get; set; }

    /// <summary>
    /// Set to <c>true</c> to allow the operation to delete child pages or databases.
    /// Defaults to <c>false</c>.
    /// </summary>
    [JsonPropertyName("allow_deleting_content")]
    public bool? AllowDeletingContent { get; set; }
}
