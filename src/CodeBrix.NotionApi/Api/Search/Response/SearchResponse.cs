using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SearchResponse : PaginatedList<ISearchResponseObject>
{
    [JsonPropertyName("page_or_database")]
    public Dictionary<string, object> PageOrDatabase { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
