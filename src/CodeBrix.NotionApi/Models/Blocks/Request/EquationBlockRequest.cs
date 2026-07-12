using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EquationBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("equation")]
    public Info Equation { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Equation;

    public class Info
    {
        [JsonPropertyName("expression")]
        public string Expression { get; set; }
    }
}
