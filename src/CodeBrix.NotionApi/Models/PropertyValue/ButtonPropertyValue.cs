using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ButtonPropertyValue : PropertyValue
{
    public override PropertyValueType Type => PropertyValueType.Button;

    [JsonPropertyName("button")]
    public ButtonValue Button { get; set; }

    public class ButtonValue { }
}
