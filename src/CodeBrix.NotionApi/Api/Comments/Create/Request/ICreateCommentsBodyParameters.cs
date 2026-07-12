using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface ICreateCommentsBodyParameters
{
    [JsonPropertyName("rich_text")]
    public IEnumerable<RichTextBaseInput> RichText { get; set; }
}
