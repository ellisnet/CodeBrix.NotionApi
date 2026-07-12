using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface ICreateDataSourceBodyParameters
{
    [JsonPropertyName("parent")]
    public IParentOfDataSourceRequest Parent { get; set; }

    [JsonPropertyName("properties")]
    public IDictionary<string, DataSourcePropertyConfigRequest> Properties { get; set; }

    [JsonPropertyName("title")]
    public IEnumerable<RichTextBaseInput> Title { get; set; }

    [JsonPropertyName("icon")]
    public IPageIconRequest Icon { get; set; }
}
