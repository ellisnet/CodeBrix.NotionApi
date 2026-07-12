using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListDataSourceTemplatesRequest : IListDataSourceTemplatesPathParameters, IListDataSourceTemplatesQueryParameters
{
    public string DataSourceId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }
    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
