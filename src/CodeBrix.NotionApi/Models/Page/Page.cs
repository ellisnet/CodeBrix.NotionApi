using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Page : IObject, IObjectModificationData, IQueryDataSourceResponseObject, ISearchResponseObject
{
    /// <summary>
    ///     The parent of this page. Can be a database, page, or workspace.
    /// </summary>
    [JsonPropertyName("parent")]
    public IParentOfPage Parent { get; set; }

    /// <summary>
    ///     Indicates whether the page is currently in the trash.
    /// </summary>
    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    /// <summary>
    ///     Whether the page is locked from editing in the Notion app UI.
    /// </summary>
    [JsonPropertyName("is_locked")]
    public bool? IsLocked { get; set; }

    /// <summary>
    ///     Property values of this page.
    /// </summary>
    [JsonPropertyName("properties")]
    public IDictionary<string, PropertyValue> Properties { get; set; }

    /// <summary>
    ///     The URL of the Notion page.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    ///     Page icon.
    /// </summary>
    [JsonPropertyName("icon")]
    public IPageIcon Icon { get; set; }

    /// <summary>
    /// The cover image of the page.
    /// </summary>
    [JsonPropertyName("cover")]
    public IPageCover Cover { get; set; }

    /// <summary>
    ///     Object type
    /// </summary>
    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.Page;

    /// <summary>
    ///     Unique identifier of the page.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Date and time when this page was created.
    /// </summary>
    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    /// <summary>
    ///     Date and time when this page was updated.
    /// </summary>
    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public PartialUser LastEditedBy { get; set; }

    /// <summary>
    ///     The public page URL if the page has been published to the web. Otherwise, null.
    /// </summary>
    [JsonPropertyName("public_url")]
    public string PublicUrl { get; set; }
}
