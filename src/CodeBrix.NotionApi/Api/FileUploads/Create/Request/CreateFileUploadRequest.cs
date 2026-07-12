using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CreateFileUploadRequest : ICreateFileUploadBodyParameters
{
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FileUploadMode Mode { get; set; }

    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("content_type")]
    public string ContentType { get; set; }

    [JsonPropertyName("number_of_parts")]
    public int? NumberOfParts { get; set; }

    [JsonPropertyName("external_url")]
    public string ExternalUrl { get; set; }
}
