using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PDFUpdateBlock : UpdateBlock
{
    [JsonPropertyName("pdf")]
    public IFileObjectInput PDF { get; set; }
}
