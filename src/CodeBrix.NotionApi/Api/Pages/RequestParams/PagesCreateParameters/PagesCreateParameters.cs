using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PagesCreateParameters : IPagesCreateBodyParameters, IPagesCreateQueryParameters
{
    [JsonPropertyName("parent")]
    public IParentOfPageRequest Parent { get; set; }

    [JsonPropertyName("properties")]
    public IDictionary<string, PropertyValue> Properties { get; set; }

    [JsonPropertyName("children")]
    public IList<IBlock> Children { get; set; }

    [JsonPropertyName("icon")]
    public IPageIconRequest Icon { get; set; }

    [JsonPropertyName("cover")]
    public IPageCoverRequest Cover { get; set; }

    [JsonPropertyName("markdown")]
    public string Markdown { get; set; }

    [JsonPropertyName("template")]
    public PageTemplate Template { get; set; }

    [JsonPropertyName("position")]
    public PagePosition Position { get; set; }
}
