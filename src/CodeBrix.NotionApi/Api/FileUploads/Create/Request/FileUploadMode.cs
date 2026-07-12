using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public enum FileUploadMode
{
    [JsonStringEnumMemberName("single_part")]
    SinglePart,

    [JsonStringEnumMemberName("multi_part")]
    MultiPart,

    [JsonStringEnumMemberName("external_url")]
    ExternalUrl
}
