using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MultiSelectDataSourcePropertyConfig : DataSourcePropertyConfig
{
    public override string Type => DataSourcePropertyTypes.MultiSelect;

    [JsonPropertyName("multi_select")]
    public OptionWrapper<SelectOptionResponse> MultiSelect { get; set; }
}
