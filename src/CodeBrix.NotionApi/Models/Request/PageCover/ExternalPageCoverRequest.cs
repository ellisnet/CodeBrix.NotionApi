using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ExternalPageCoverRequest : IPageCoverRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageCoverRequestTypes.External;

    [JsonPropertyName("external")]
    public Info External { get; set; }

    public class Info
    {
        /// <summary>
        /// The URL of the external file.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
