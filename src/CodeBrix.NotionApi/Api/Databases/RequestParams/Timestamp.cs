using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum Timestamp
{
    Unknown,

    [JsonStringEnumMemberName("created_time")]
    CreatedTime,

    [JsonStringEnumMemberName("last_edited_time")]
    LastEditedTime
}
