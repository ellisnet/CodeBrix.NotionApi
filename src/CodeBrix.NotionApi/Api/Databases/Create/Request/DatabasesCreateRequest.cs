using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class DatabasesCreateRequest : IDatabasesCreateBodyParameters
{
    [JsonPropertyName("parent")]
    public IParentOfDatabaseRequest Parent { get; set; }

    [JsonPropertyName("title")]
    public List<RichTextBaseInput> Title { get; set; }

    [JsonPropertyName("description")]
    public List<RichTextBaseInput> Description { get; set; }

    [JsonPropertyName("is_inline")]
    public bool? IsInline { get; set; }

    [JsonPropertyName("initial_data_source")]
    public InitialDataSourceRequest InitialDataSource { get; set; }

    public IPageIconRequest Icon { get; set; }

    public IPageCoverRequest Cover { get; set; }
}

public class InitialDataSourceRequest
{
    [JsonPropertyName("properties")]
    public Dictionary<string, DataSourcePropertyConfigRequest> Properties { get; set; }
}
