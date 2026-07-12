using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum SearchDirection
{
    [JsonStringEnumMemberName("ascending")]
    Ascending,

    [JsonStringEnumMemberName("descending")]
    Descending
}
