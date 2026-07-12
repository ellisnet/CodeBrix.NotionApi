using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class VerificationProperty : Property
{
    public override PropertyType Type => PropertyType.Verification;

    [JsonPropertyName("verification")]
    public Dictionary<string, object> Verification { get; set; }
}
