using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PDFBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("pdf")]
    public FileObject PDF { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.PDF;
}
