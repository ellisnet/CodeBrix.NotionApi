using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class View : IObject
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public ObjectType Object => ObjectType.View;

    [JsonPropertyName("type")]
    public ViewType Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("data_source_id")]
    public string DataSourceId { get; set; }

    [JsonPropertyName("parent")]
    public DatabaseParent Parent { get; set; }

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("created_by")]
    public PartialUser CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public PartialUser LastEditedBy { get; set; }

    [JsonPropertyName("filter")]
    public object Filter { get; set; }

    [JsonPropertyName("sorts")]
    public IEnumerable<ViewSort> Sorts { get; set; }

    [JsonPropertyName("configuration")]
    public ViewConfiguration Configuration { get; set; }
}
