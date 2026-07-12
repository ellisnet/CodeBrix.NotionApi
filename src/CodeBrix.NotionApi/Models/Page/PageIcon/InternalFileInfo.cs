using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class InternalFileInfo
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("expiry_time")]
    public DateTime ExpiryTime { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
