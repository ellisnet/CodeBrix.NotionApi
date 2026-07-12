using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UploadedFileInput : IFileObjectInput
{
    [JsonPropertyName("file")]
    public Data File { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("caption")]
    public IEnumerable<RichTextBaseInput> Caption { get; set; }

    public class Data
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("expiry_time")]
        public DateTime ExpiryTime { get; set; }
    }
}
