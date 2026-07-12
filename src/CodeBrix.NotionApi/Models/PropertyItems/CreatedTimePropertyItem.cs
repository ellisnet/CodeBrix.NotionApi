using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreatedTimePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "created_time";

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }
}
