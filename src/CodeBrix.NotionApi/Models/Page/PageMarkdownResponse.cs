using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PageMarkdownResponse : IObject
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.PageMarkdown;

    /// <summary>
    /// The page content rendered as enhanced Markdown.
    /// </summary>
    [JsonPropertyName("markdown")]
    public string Markdown { get; set; }

    /// <summary>
    /// Whether the content was truncated due to exceeding the record count limit.
    /// </summary>
    [JsonPropertyName("truncated")]
    public bool Truncated { get; set; }

    /// <summary>
    /// Block IDs that could not be loaded (appeared as <unknown> tags in the markdown). 
    /// Pass these IDs back to this endpoint to fetch their content separately.
    /// </summary>
    [JsonPropertyName("unknown_block_ids")]
    public IEnumerable<string> UnknownBlockIds { get; set; }
}
