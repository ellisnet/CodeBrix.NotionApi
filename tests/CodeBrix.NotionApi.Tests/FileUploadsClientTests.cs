using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class FileUploadsClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly FileUploadsClient _fileUploadClient;

    public FileUploadsClientTests()
    {
        _fileUploadClient = new FileUploadsClient(_restClient);
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fileUploadClient.CreateAsync(null, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("fileUploadObjectRequest", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'fileUploadObjectRequest')", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_CallsRestClientPostAsync_WithCorrectParameters()
    {
        // Arrange
        var request = new CreateFileUploadRequest
        {
            FileName = "testfile.txt",
            Mode = FileUploadMode.SinglePart,
        };

        var expectedResponse = new FileUpload
        {
            UploadUrl = "https://example.com/upload",
            Id = Guid.NewGuid().ToString(),
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var response = await _fileUploadClient.CreateAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResponse.UploadUrl, response.UploadUrl);
        Assert.Equal(expectedResponse.Id, response.Id);

        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.FileUploadsApiUrls.Create(), _restClient.LastCall.Uri);
        Assert.IsAssignableFrom<ICreateFileUploadBodyParameters>(_restClient.LastCall.Body);
    }

    [Fact]
    public async Task SendAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fileUploadClient.SendAsync(null, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("sendFileUploadRequest", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'sendFileUploadRequest')", exception.Message);
    }

    [Fact]
    public async Task SendAsync_ThrowsArgumentNullException_WhenFileUploadIdIsNullOrEmpty()
    {
        // Arrange
        var request = SendFileUploadRequest.Create(fileUploadId: null, file: new FileData { FileName = "testfile.txt", Data = new System.IO.MemoryStream(), ContentType = "text/plain" });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fileUploadClient.SendAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("FileUploadId", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'FileUploadId')", exception.Message);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1001")]
    [InlineData("-5")]
    [InlineData("abc")]
    public async Task SendAsync_ThrowsArgumentOutOfRangeException_WhenPartNumberIsInvalid(string partNumber)
    {
        // Arrange
        var request = SendFileUploadRequest.Create(fileUploadId: "valid-id", file: new FileData { FileName = "testfile.txt", Data = new System.IO.MemoryStream(), ContentType = "text/plain" }, partNumber: partNumber);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _fileUploadClient.SendAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("PartNumber", exception.ParamName);
        Assert.Contains("PartNumber must be between 1 and 1000.", exception.Message);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("500")]
    [InlineData("1000")]
    public async Task SendAsync_DoesNotThrow_WhenPartNumberIsValid(string partNumber)
    {
        // Arrange
        var request = SendFileUploadRequest.Create(fileUploadId: "valid-id", file: new FileData { FileName = "testfile.txt", Data = new System.IO.MemoryStream(), ContentType = "text/plain" }, partNumber: partNumber);

        var expectedResponse = new FileUpload
        {
            Id = "valid-id",
            Status = "uploaded",
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var exception = await Record.ExceptionAsync(() => _fileUploadClient.SendAsync(request, cancellationToken: TestContext.Current.CancellationToken));

        // Assert
        Assert.Null(exception);
        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.FileUploadsApiUrls.Send("valid-id"), _restClient.LastCall.Uri);
        Assert.NotNull(_restClient.LastCall.FormData);
    }

    [Fact]
    public async Task SendAsync_CallsRestClientPostAsync_WithCorrectParameters()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid().ToString();
        var request = SendFileUploadRequest.Create(
            fileUploadId: fileUploadId,
            file: new FileData
            {
                FileName = "testfile.txt",
                Data = new System.IO.MemoryStream(),
                ContentType = "text/plain"
            }
        );

        var expectedResponse = new FileUpload
        {
            Id = fileUploadId.ToString(),
            Status = "uploaded",
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var response = await _fileUploadClient.SendAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResponse.Status, response.Status);
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.FileUploadsApiUrls.Send(fileUploadId), _restClient.LastCall.Uri);
        Assert.NotNull(_restClient.LastCall.FormData);
    }

    #region ListAsync Tests

    [Fact]
    public async Task ListAsync_CallsRestClientGetAsync_WithCorrectParameters()
    {
        // Arrange
        var request = new ListFileUploadsRequest
        {
            PageSize = 10,
            StartCursor = "cursor123",
            Status = "completed"
        };

        _restClient.SetResponse(new ListFileUploadsResponse
        {
            Results = new List<FileUpload>
            {
                new() { Id = "file1", Status = "completed" },
                new() { Id = "file2", Status = "completed" }
            }
        });

        // Act
        var response = await _fileUploadClient.ListAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Results.Count);
        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.FileUploadsApiUrls.List, _restClient.LastCall.Uri);
    }

    #endregion ListAsync Tests

    #region RetrieveAsync Tests

    // Add tests for RetrieveAsync method
    [Fact]
    public async Task RetrieveAsync_ThrowsArgumentNullException_WhenFileUploadIdIsNullOrEmpty()
    {
        // Arrange
        var request = new RetrieveFileUploadRequest
        {
            FileUploadId = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fileUploadClient.RetrieveAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("FileUploadId", exception.ParamName);
        Assert.Equal("FileUploadId cannot be null or empty. (Parameter 'FileUploadId')", exception.Message);
    }

    [Fact]
    public async Task RetrieveAsync_CallsRestClientGetAsync_WithCorrectParameters()
    {
        // Arrange
        var fileUploadId = Guid.NewGuid().ToString();
        var request = new RetrieveFileUploadRequest
        {
            FileUploadId = fileUploadId
        };

        var expectedResponse = new FileUpload
        {
            Id = fileUploadId,
            FileName = "testfile.txt",
            Status = "completed"
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var response = await _fileUploadClient.RetrieveAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResponse.Id, response.Id);
        Assert.Equal(expectedResponse.FileName, response.FileName);
        Assert.Equal(expectedResponse.Status, response.Status);
        Assert.Single(_restClient.Calls);
        Assert.Equal(ApiEndpoints.FileUploadsApiUrls.Retrieve(request), _restClient.LastCall.Uri);
    }

    #endregion RetrieveAsync Tests
}
