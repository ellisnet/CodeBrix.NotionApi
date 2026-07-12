using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IPagesCreateBodyParameters
{
    [JsonPropertyName("parent")]
    IParentOfPageRequest Parent { get; set; }

    [JsonPropertyName("properties")]
    IDictionary<string, PropertyValue> Properties { get; set; }

    [JsonPropertyName("children")]
    IList<IBlock> Children { get; set; }

    [JsonPropertyName("icon")]
    IPageIconRequest Icon { get; set; }

    [JsonPropertyName("cover")]
    IPageCoverRequest Cover { get; set; }

    [JsonPropertyName("markdown")]
    string Markdown { get; set; }

    /// <summary>
    /// Template to apply when creating the page.
    /// Use <see cref="NonePageTemplate"/>, <see cref="DefaultPageTemplate"/>, or <see cref="TemplateIdPageTemplate"/>.
    /// Mutually exclusive with <see cref="Children"/> and <see cref="Markdown"/>.
    /// </summary>
    [JsonPropertyName("template")]
    PageTemplate Template { get; set; }

    /// <summary>
    /// Controls where the new page is placed within its parent.
    /// Use <see cref="AfterBlockPagePosition"/>, <see cref="PageStartPosition"/>, or <see cref="PageEndPosition"/>.
    /// </summary>
    [JsonPropertyName("position")]
    PagePosition Position { get; set; }
}
