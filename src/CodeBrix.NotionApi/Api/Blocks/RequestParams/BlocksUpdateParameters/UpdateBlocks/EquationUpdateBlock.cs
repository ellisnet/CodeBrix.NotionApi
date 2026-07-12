using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EquationUpdateBlock : UpdateBlock
{
    [JsonPropertyName("equation")]
    public Info Equation { get; set; }

    public class Info
    {
        [JsonPropertyName("expression")]
        public string Expression { get; set; }
    }
}
