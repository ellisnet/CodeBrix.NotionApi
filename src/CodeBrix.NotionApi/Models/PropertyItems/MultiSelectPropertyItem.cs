using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MultiSelectPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "multi_select";

    [JsonPropertyName("multi_select")]
    public IEnumerable<SelectOption> MultiSelect { get; set; }
}
