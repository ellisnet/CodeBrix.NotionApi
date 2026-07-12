using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum Direction
{
    Unknown,

    [JsonStringEnumMemberName("ascending")]
    Ascending,

    [JsonStringEnumMemberName("descending")]
    Descending
}
