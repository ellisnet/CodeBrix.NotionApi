using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface ICreateFileUploadBodyParameters
{
    /// <summary>
    /// How the file is being sent. Use multi_part for files larger than 20MB. 
    /// Use external_url for files that are temporarily hosted publicly elsewhere. 
    /// Default is single_part.
    /// </summary>
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    FileUploadMode Mode { get; }

    /// <summary>
    /// Name of the file to be created. Required when mode is multi_part or external_url. 
    /// Otherwise optional, and used to override the filename. Must include an extension, or have one inferred 
    /// from the content_type parameter.
    /// </summary>
    [JsonPropertyName("filename")]
    string FileName { get; }

    /// <summary>
    /// MIME type of the file to be created. Recommended when sending the file in multiple parts. 
    /// Must match the content type of the file that's sent, and the extension of the filename parameter if any.
    /// </summary>
    [JsonPropertyName("content_type")]
    string ContentType { get; }

    /// <summary>
    /// When mode is multi_part, the number of parts you are uploading. 
    /// Must be between 1 and 1,000. This must match the number of parts as well as the final part_number you send.
    /// </summary>
    [JsonPropertyName("number_of_parts")]
    int? NumberOfParts { get; }

    /// <summary>
    /// When mode is external_url, provide the HTTPS URL of a publicly accessible file to import into your workspace.
    /// </summary>
    [JsonPropertyName("external_url")]
    string ExternalUrl { get; }
}
