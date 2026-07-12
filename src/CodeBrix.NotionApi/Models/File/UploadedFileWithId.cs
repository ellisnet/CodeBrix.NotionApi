using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UploadedFileWithId : FileObjectWithName
{
    public override string Type => "file_upload";
    
    [JsonPropertyName("file_upload")]
    public Info FileUpload { get; set; }

    public class Info
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
