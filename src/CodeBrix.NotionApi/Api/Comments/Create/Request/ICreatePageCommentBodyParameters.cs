using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface ICreatePageCommentBodyParameters : ICreateCommentsBodyParameters
{
    [JsonPropertyName("parent")]
    public ParentPageInput Parent { get; set; }
}
