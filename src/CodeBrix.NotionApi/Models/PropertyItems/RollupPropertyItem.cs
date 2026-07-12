using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RollupPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "rollup";

    [JsonPropertyName("rollup")]
    public Data Rollup { get; set; }

    public class Data
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("function")]
        public string Function { get; set; }

        [JsonPropertyName("number")]
        public double? Number { get; set; }

        [JsonPropertyName("date")]
        public Date Date { get; set; }

        [JsonPropertyName("array")]
        public IEnumerable<Dictionary<string, object>> Array { get; set; }

        [JsonPropertyName("unsupported")]
        public Dictionary<string, object> Unsupported { get; set; }

        [JsonPropertyName("incomplete")]
        public Dictionary<string, object> Incomplete { get; set; }
    }
}
