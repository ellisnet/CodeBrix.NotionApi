using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IPagesUpdateBodyParameters
{
    [JsonPropertyName("icon")]
    IPageIconRequest Icon { get; set; }

    [JsonPropertyName("cover")]
    IPageCoverRequest Cover { get; set; }

    [JsonPropertyName("in_trash")]
    bool InTrash { get; set; }

    [JsonPropertyName("properties")]
    IDictionary<string, PropertyValue> Properties { get; set; }

    /// <summary>
    /// Template to apply to the page. Use <see cref="DefaultPageTemplate"/> or <see cref="TemplateIdPageTemplate"/>.
    /// </summary>
    [JsonPropertyName("template")]
    PageTemplate Template { get; set; }

    /// <summary>
    /// When <c>true</c>, erases all existing content from the page before applying the template (if any).
    /// </summary>
    [JsonPropertyName("erase_content")]
    bool? EraseContent { get; set; }

    /// <summary>
    /// Whether the page should be locked from editing in the Notion app UI.
    /// If not provided, the locked state will not be updated.
    /// </summary>
    [JsonPropertyName("is_locked")]
    bool? IsLocked { get; set; }
}
