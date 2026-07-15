using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(RichTextText), RichTextType.TextValue)]
[JsonKnownType(typeof(RichTextEquation), RichTextType.EquationValue)]
[JsonKnownType(typeof(RichTextMention), RichTextType.MentionValue)]
[JsonFallbackType(typeof(UnknownRichText))]
public class RichTextBase
{
    [JsonPropertyName("plain_text")]
    public string PlainText { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }

    [JsonPropertyName("annotations")]
    public Annotations Annotations { get; set; }

    [JsonPropertyName("type")]
    public virtual RichTextType Type { get; set; }
}

public class Annotations
{
    [JsonPropertyName("bold")]
    public bool IsBold { get; set; }

    [JsonPropertyName("italic")]
    public bool IsItalic { get; set; }

    [JsonPropertyName("strikethrough")]
    public bool IsStrikeThrough { get; set; }

    [JsonPropertyName("underline")]
    public bool IsUnderline { get; set; }

    [JsonPropertyName("code")]
    public bool IsCode { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }
}
