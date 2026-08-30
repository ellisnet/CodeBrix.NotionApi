using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DataSource : IObject, IObjectModificationData, IQueryDataSourceResponseObject, ISearchResponseObject
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.DataSource;

    /// <summary>
    /// The title of the data source.
    /// </summary>
    [JsonPropertyName("title")]
    public IEnumerable<RichTextBase> Title { get; set; }

    /// <summary>
    /// A description of the data source.
    /// </summary>
    [JsonPropertyName("description")]
    public IEnumerable<RichTextBase> Description { get; set; }

    /// <summary>
    /// The parent of the data source.
    /// </summary>
    [JsonPropertyName("parent")]
    public IParentOfDatasource Parent { get; set; }

    /// <summary>
    /// The parent of the data source's containing database. This is typically a page, block,
    /// or workspace, but can be another database in the case of wikis.
    /// </summary>
    [JsonPropertyName("database_parent")]
    public IParentOfDatabase DatabaseParent { get; set; }

    /// <summary>
    /// Indicates whether the data source is inline within its parent database.
    /// </summary>
    [JsonPropertyName("is_inline")]
    public bool IsInline { get; set; }

    [Obsolete("Use InTrash instead. The 'archived' field is deprecated as of Notion API version 2026-03-11.")]
    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    /// <summary>
    /// Indicates whether the data source is in the trash.
    /// </summary>
    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public PartialUser LastEditedBy { get; set; }

    /// <summary>
    /// The properties schema of the data source.
    /// </summary>
    [JsonPropertyName("properties")]
    public IDictionary<string, DataSourcePropertyConfig> Properties { get; set; }

    /// <summary>
    /// The icon of the data source.
    /// </summary>
    [JsonPropertyName("icon")]
    public IPageIcon Icon { get; set; }

    /// <summary>
    /// The cover image of the data source.
    /// </summary>
    [JsonPropertyName("cover")]
    public IPageCover Cover { get; set; }

    /// <summary>
    /// The URL of the data source.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// The public page URL if the data source has been published to the web. Otherwise, null.
    /// </summary>
    [JsonPropertyName("public_url")]
    public string PublicUrl { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
