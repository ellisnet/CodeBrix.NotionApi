using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextText : RichTextBase
{
    public override RichTextType Type => RichTextType.Text;

    [JsonPropertyName("text")]
    public Text Text { get; set; }
}

public class Text
{
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("link")]
    public Link Link { get; set; }
}

public class Link
{
    [JsonPropertyName("type")]
    public string Type => "url";

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
