using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IUpdateDataSourceBodyParameters
{
    /// <summary>
    /// The title of the data source.
    /// </summary>
    IEnumerable<RichTextBaseInput> Title { get; set; }

    /// <summary>
    /// Page icon for data source.
    /// </summary>
    IPageIconRequest Icon { get; set; }

    IDictionary<string, IUpdatePropertyConfigurationRequest> Properties { get; set; }

    /// <summary>
    /// Whether the database should be moved to or from the trash. If not provided, the trash
    /// status will not be updated.
    /// </summary>
    [JsonPropertyName("in_trash")]
    bool InTrash { get; set; }

    /// <summary>
    /// Whether the database should be moved to or from the trash. If not provided, the trash
    /// status will not be updated. Equivalent to `in_trash`.
    /// </summary>
    [JsonPropertyName("archived")]
    bool Archived { get; set; }

    /// <summary>
    /// The parent of the data source.
    /// </summary>
    [JsonPropertyName("parent")]
    IParentOfDataSourceRequest Parent { get; set; }
}
