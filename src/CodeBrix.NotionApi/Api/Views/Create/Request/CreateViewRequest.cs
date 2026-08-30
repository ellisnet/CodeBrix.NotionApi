using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreateViewRequest
{
    [JsonPropertyName("data_source_id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public ViewType Type { get; set; }

    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; }

    [JsonPropertyName("filter")]
    public object Filter { get; set; }

    [JsonPropertyName("sorts")]
    public IEnumerable<ViewSort> Sorts { get; set; }

    [JsonPropertyName("configuration")]
    public ViewConfiguration Configuration { get; set; }
}
