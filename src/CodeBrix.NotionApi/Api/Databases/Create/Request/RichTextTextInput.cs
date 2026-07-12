using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextTextInput : RichTextBaseInput
{
    public override RichTextType Type => RichTextType.Text;

    [JsonPropertyName("text")]
    public Text Text { get; set; }
}
