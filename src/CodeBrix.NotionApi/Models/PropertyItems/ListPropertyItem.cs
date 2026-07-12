using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ListPropertyItem : IPropertyItemObject
{
    [JsonPropertyName("results")]
    public IEnumerable<SimplePropertyItem> Results { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("next_cursor")]
    public string NextCursor { get; set; }

    [JsonPropertyName("property_item")]
    public SimplePropertyItem PropertyItem { get; set; }

    [JsonPropertyName("object")]
    public string Object => "list";

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("next_url")]
    public string NextURL { get; set; }
}
