using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DatabasesUpdateRequest : IDatabasesUpdatePathParameters, IDatabasesUpdateBodyParameters
{
    [JsonIgnore]
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }

    [JsonPropertyName("title")]
    public List<RichTextBaseInput> Title { get; set; }

    [JsonPropertyName("icon")]
    public IPageIconRequest Icon { get; set; }

    [JsonPropertyName("cover")]
    public IPageCoverRequest Cover { get; set; }

    [JsonPropertyName("in_trash")]
    public bool? InTrash { get; set; }

    [JsonPropertyName("is_inline")]
    public bool? IsInline { get; set; }

    [JsonPropertyName("description")]
    public List<RichTextBaseInput> Description { get; set; }

    [JsonPropertyName("is_locked")]
    public bool? IsLocked { get; set; }

    [JsonPropertyName("parent")]
    public IParentOfDatabaseRequest Parent { get; set; }
}

public interface IDatabasesUpdatePathParameters
{
    /// <summary>
    /// The ID of the database to update.
    /// </summary>
    [JsonIgnore]
    [JsonPropertyName("database_id")]
    string DatabaseId { get; set; }
}
