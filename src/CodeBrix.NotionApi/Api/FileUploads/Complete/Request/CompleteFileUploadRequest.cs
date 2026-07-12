namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class CompleteFileUploadRequest : ICompleteFileUploadPathParameters
{
    public string FileUploadId { get; set; }
}
