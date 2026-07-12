using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(EmojiPageIcon), PageIconTypes.Emoji)]
[JsonKnownType(typeof(CustomEmojiPageIcon), PageIconTypes.CustomEmoji)]
[JsonKnownType(typeof(FilePageIcon), PageIconTypes.File)]
[JsonKnownType(typeof(ExternalPageIcon), PageIconTypes.External)]
[JsonKnownType(typeof(IconPageIcon), PageIconTypes.Icon)]
[JsonFallbackType(typeof(ExternalPageIcon))]
public interface IPageIcon
{
    [JsonPropertyName("type")]
    string Type { get; set; }
}
