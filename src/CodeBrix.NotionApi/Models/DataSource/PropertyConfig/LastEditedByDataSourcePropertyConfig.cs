using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LastEditedByDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.LastEditedBy;

    [JsonPropertyName("last_edited_by")]
    public Dictionary<string, object> LastEditedBy { get; set; }
}
