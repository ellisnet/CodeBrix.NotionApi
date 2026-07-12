using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextMentionInput : RichTextBaseInput
{
    public override RichTextType Type => RichTextType.Mention;

    [JsonPropertyName("mention")]
    public MentionInput Mention { get; set; }
}
