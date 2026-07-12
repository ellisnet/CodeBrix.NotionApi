using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class MultiSelectDataSourcePropertyConfigRequest : DataSourcePropertyConfigRequest
{
    [JsonPropertyName("type")]
    public override string Type => "multi_select";

    [JsonPropertyName("multi_select")]
    public MultiSelectOptions MultiSelect { get; set; }

    public class MultiSelectOptions
    {
        [JsonPropertyName("options")]
        public IEnumerable<SelectOptionRequest> Options { get; set; }

        /// <summary>
        /// Additional data for future compatibility
        /// If you encounter properties that are not yet supported, please open an issue on GitHub.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
