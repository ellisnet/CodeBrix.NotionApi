using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IDatabasesUpdateBodyParameters
{
    [JsonPropertyName("parent")]
    IParentOfDatabaseRequest Parent { get; set; }

    [JsonPropertyName("title")]
    List<RichTextBaseInput> Title { get; set; }

    [JsonPropertyName("icon")]
    IPageIconRequest Icon { get; set; }

    [JsonPropertyName("cover")]
    IPageCoverRequest Cover { get; set; }

    [JsonPropertyName("in_trash")]
    bool? InTrash { get; set; }

    [JsonPropertyName("is_inline")]
    bool? IsInline { get; set; }

    [JsonPropertyName("description")]
    List<RichTextBaseInput> Description { get; set; }

    [JsonPropertyName("is_locked")]
    bool? IsLocked { get; set; }
}
