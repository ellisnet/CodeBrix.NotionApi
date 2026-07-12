using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public enum RelationType
{
    [JsonStringEnumMemberName("single_property")]
    Single,

    [JsonStringEnumMemberName("dual_property")]
    Dual
}
