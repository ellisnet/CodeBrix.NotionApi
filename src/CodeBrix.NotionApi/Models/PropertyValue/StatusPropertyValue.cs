using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
///     Status property value objects contain page status
/// </summary>
public class StatusPropertyValue : PropertyValue
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public enum StatusColor
    {
        [JsonStringEnumMemberName("default")]
        Default,

        [JsonStringEnumMemberName("gray")]
        Gray,

        [JsonStringEnumMemberName("brown")]
        Brown,

        [JsonStringEnumMemberName("orange")]
        Orange,

        [JsonStringEnumMemberName("yellow")]
        Yellow,

        [JsonStringEnumMemberName("green")]
        Green,

        [JsonStringEnumMemberName("blue")]
        Blue,

        [JsonStringEnumMemberName("purple")]
        Purple,

        [JsonStringEnumMemberName("pink")]
        Pink,

        [JsonStringEnumMemberName("red")]
        Red
    }

    public override PropertyValueType Type => PropertyValueType.Status;

    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Data Status { get; set; }

    public class Data
    {
        /// <summary>
        ///     ID of the option.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        ///     Name of the option as it appears in Notion.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Color of the option.
        /// </summary>
        [JsonPropertyName("color")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusColor? Color { get; set; }
    }
}
