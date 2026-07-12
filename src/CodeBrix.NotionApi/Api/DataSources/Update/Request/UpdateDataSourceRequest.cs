using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UpdateDataSourceRequest : IUpdateDataSourcePathParameters, IUpdateDataSourceBodyParameters
{
    public string DataSourceId { get; set; }
    public IEnumerable<RichTextBaseInput> Title { get; set; }
    public IPageIconRequest Icon { get; set; }
    public IDictionary<string, IUpdatePropertyConfigurationRequest> Properties { get; set; }
    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }
    [JsonPropertyName("archived")]
    public bool Archived { get; set; }
    [JsonPropertyName("parent")]
    public IParentOfDataSourceRequest Parent { get; set; }
}
