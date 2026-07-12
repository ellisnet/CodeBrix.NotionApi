using System;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UploadedFile : FileObject
{
    public override string Type => "file";

    [JsonPropertyName("file")]
    public Info File { get; set; }

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("expiry_time")]
        public DateTime ExpiryTime { get; set; }
    }
}
