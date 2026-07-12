using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListFileUploadsResponse : PaginatedList<FileUpload>
{
    [JsonPropertyName("file_uploads")]
    public Dictionary<string, object> FileUploads { get; set; }
}
