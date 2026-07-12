namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrieveFileUploadRequest : IRetrieveFileUploadPathParameters
{
    public string FileUploadId { get; set; }
}
