using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileUploadPageCoverRequest : IPageCoverRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageCoverRequestTypes.FileUpload;

    [JsonPropertyName(PageCoverRequestTypes.FileUpload)]
    public Info File { get; set; }

    public class Info
    {
        /// <summary>
        /// The ID of a FileUpload object that has the status `uploaded`.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
