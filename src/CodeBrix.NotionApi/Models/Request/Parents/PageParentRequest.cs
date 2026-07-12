using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class PageParentRequest : IParentOfPageRequest
{
    [JsonPropertyName("type")]
    public string Type => "page_id";

    [JsonPropertyName("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// Additional data for future compatibility
    /// If you encounter properties that are not yet supported, please open an issue on GitHub.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
