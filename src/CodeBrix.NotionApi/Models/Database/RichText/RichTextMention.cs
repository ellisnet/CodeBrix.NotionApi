using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RichTextMention : RichTextBase
{
    public override RichTextType Type => RichTextType.Mention;

    [JsonPropertyName("mention")]
    public Mention Mention { get; set; }
}

public class Mention
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("page")]
    public ObjectId Page { get; set; }

    [JsonPropertyName("database")]
    public ObjectId Database { get; set; }

    [JsonPropertyName("date")]
    public Date Date { get; set; }
}

public class ObjectId
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
