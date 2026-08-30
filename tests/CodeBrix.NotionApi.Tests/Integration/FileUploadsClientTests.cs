using System;
using System.IO;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;
[Collection(NotionIntegrationCollection.Name)]
public class FileUploadsClientTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateAsync()
    {
        // Arrange
        var request = new CreateFileUploadRequest
        {
            Mode = FileUploadMode.ExternalUrl,
            ExternalUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg",
            FileName = "sample-image.jpg",
        };

        // Act
        var response = await Client.FileUploads.CreateAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Status);
        Assert.Equal("sample-image.jpg", response.FileName);
    }

    [Fact]
    public async Task Verify_file_upload_flow()
    {
        // Arrange
        var createRequest = new CreateFileUploadRequest
        {
            Mode = FileUploadMode.SinglePart,
            FileName = "notion-logo.png",
        };

        var createResponse = await Client.FileUploads.CreateAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        using (var fileStream = File.OpenRead("assets/notion-logo.png"))
        {
            var sendRequest = SendFileUploadRequest.Create(
                createResponse.Id,
                new FileData
                {
                    FileName = "notion-logo.png",
                    Data = fileStream,
                    ContentType = createResponse.ContentType
                }
            );

            // Act
            var sendResponse = await Client.FileUploads.SendAsync(sendRequest, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.NotNull(sendResponse);
            Assert.Equal(createResponse.Id, sendResponse.Id);
            Assert.Equal("notion-logo.png", sendResponse.FileName);
            Assert.Equal("uploaded", sendResponse.Status);
        }
    }

    [Fact]
    public async Task Verify_multi_part_file_upload_flow()
    {
        // Arrange - Notion requires every part except the last to be at least 5 MiB, so the 4.56 KiB
        // notion-logo.png asset cannot drive this flow. Build a large payload in memory instead of
        // committing a multi-megabyte fixture.
        const string FileName = "multipart-sample.txt";
        const int PartSizeInBytes = 6 * 1024 * 1024;

        var payload = new byte[PartSizeInBytes * 2];
        var line = System.Text.Encoding.ASCII.GetBytes("CodeBrix.NotionApi multi-part upload test payload.\n");

        for (var offset = 0; offset < payload.Length; offset += line.Length)
        {
            Array.Copy(line, 0, payload, offset, Math.Min(line.Length, payload.Length - offset));
        }

        var createRequest = new CreateFileUploadRequest
        {
            Mode = FileUploadMode.MultiPart,
            FileName = FileName,
            NumberOfParts = 2
        };

        var createResponse = await Client.FileUploads.CreateAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(createResponse);
        Assert.NotNull(createResponse.Id);
        Assert.Equal(FileName, createResponse.FileName);
        Assert.Equal("pending", createResponse.Status);

        // Act - send both parts
        using (var payloadStream = new MemoryStream(payload))
        {
            var splitStreams = StreamSplitExtensions.Split(payloadStream, 2);

            foreach (var (partStream, index) in splitStreams.WithIndex())
            {
                var partSendRequest = SendFileUploadRequest.Create(
                    createResponse.Id,
                    new FileData
                    {
                        FileName = FileName,
                        Data = partStream,
                        ContentType = createResponse.ContentType
                    },

                    partNumber: (index + 1).ToString()
                );

                var partSendResponse = await Client.FileUploads.SendAsync(partSendRequest, cancellationToken: TestContext.Current.CancellationToken);

                Assert.NotNull(partSendResponse);
                Assert.Equal(createResponse.Id, partSendResponse.Id);
                Assert.Equal(FileName, partSendResponse.FileName);
            }
        }

        // Assert - completing the upload flips it to "uploaded"
        var completeRequest = new CompleteFileUploadRequest
        {
            FileUploadId = createResponse.Id
        };

        var completeResponse = await Client.FileUploads.CompleteAsync(completeRequest, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(completeResponse);
        Assert.Equal(createResponse.Id, completeResponse.Id);
        Assert.Equal(FileName, completeResponse.FileName);
        Assert.Equal("uploaded", completeResponse.Status);
    }

    [Fact]
    public async Task ListAsync()
    {
        // Arrange
        var request = new ListFileUploadsRequest
        {
            PageSize = 5
        };

        // Act
        var response = await Client.FileUploads.ListAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Results);
        Assert.True(response.Results.Count <= 5);
    }

    [Fact]
    public async Task RetrieveAsync()
    {
        // Arrange
        var createRequest = new CreateFileUploadRequest
        {
            Mode = FileUploadMode.ExternalUrl,
            ExternalUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg",
            FileName = "sample-image.jpg",
        };

        var createResponse = await Client.FileUploads.CreateAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(createResponse);
        Assert.NotNull(createResponse.Id);
        Assert.Equal("sample-image.jpg", createResponse.FileName);
        Assert.Equal("image/jpeg", createResponse.ContentType);
        Assert.Equal("pending", createResponse.Status);

        // Act
        var retrieveRequest = new RetrieveFileUploadRequest
        {
            FileUploadId = createResponse.Id
        };

        var retrieveResponse = await Client.FileUploads.RetrieveAsync(retrieveRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(retrieveResponse);
        Assert.Equal(createResponse.Id, retrieveResponse.Id);
        Assert.Equal("sample-image.jpg", retrieveResponse.FileName);
        Assert.Equal("image/jpeg", retrieveResponse.ContentType);
        // The status might have changed from "pending" to "uploaded" depending on Notion's processing time
        Assert.Contains(retrieveResponse.Status, new[] { "pending", "uploaded" });
    }
}
