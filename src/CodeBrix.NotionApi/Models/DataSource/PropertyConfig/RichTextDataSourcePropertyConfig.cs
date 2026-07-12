using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.RichText;

    [JsonPropertyName("rich_text")]
    public Dictionary<string, object> RichText { get; set; }
}
