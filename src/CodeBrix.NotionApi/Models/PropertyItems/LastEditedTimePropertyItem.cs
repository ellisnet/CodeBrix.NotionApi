using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedTimePropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "last_edited_time";

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }
}
