using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class Database : IObject, IObjectModificationData
{
    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.Database;

    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The title of the database.
    /// </summary>
    [JsonPropertyName("title")]
    public List<RichTextBase> Title { get; set; }

    /// <summary>
    /// The description of the database.
    /// </summary>
    [JsonPropertyName("description")]
    public IEnumerable<RichTextBase> Description { get; set; }

    /// <summary>
    /// Parent of the database.
    /// </summary>
    [JsonPropertyName("parent")]
    public IParentOfDatabase Parent { get; set; }

    [JsonPropertyName("is_inline")]
    public bool IsInline { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    [JsonPropertyName("is_locked")]
    public bool IsLocked { get; set; }

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public PartialUser LastEditedBy { get; set; }

    /// <summary>
    /// The data sources associated with the database.
    /// </summary>
    [JsonPropertyName("data_sources")]
    public IEnumerable<DataSourceReferenceResponse> DataSources { get; set; }

    [JsonPropertyName("icon")]
    public IPageIcon Icon { get; set; }

    /// <summary>
    /// The cover image of the database.
    /// </summary>
    [JsonPropertyName("cover")]
    public IPageCover Cover { get; set; }

    /// <summary>
    ///     The URL of the Notion database.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// The public page URL if the page has been published to the web. Otherwise, null.
    /// </summary>
    [JsonPropertyName("public_url")]
    public string PublicUrl { get; set; }
}
