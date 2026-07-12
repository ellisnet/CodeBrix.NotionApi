using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedByDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "last_edited_by";

    [JsonPropertyName("last_edited_by")]
    public IDictionary<string, object> LastEditedBy { get; set; }
}
