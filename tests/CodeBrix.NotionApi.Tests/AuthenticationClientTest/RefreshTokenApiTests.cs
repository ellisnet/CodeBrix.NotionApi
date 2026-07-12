using System;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests.AuthenticationClientTest;

public class RefreshTokenApiTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly AuthenticationClient _authenticationClient;

    public RefreshTokenApiTests()
    {
        _authenticationClient = new AuthenticationClient(_restClient);
    }

    [Fact]
    public async Task RefreshTokenAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticationClient.RefreshTokenAsync(null, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("refreshTokenRequest", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'refreshTokenRequest')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RefreshTokenAsync_ThrowsArgumentNullException_WhenRefreshTokenIsNullOrEmpty(string refreshToken)
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            RefreshToken = refreshToken,
            ClientId = "validClientId",
            ClientSecret = "validClientSecret"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticationClient.RefreshTokenAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("RefreshToken", exception.ParamName);
        Assert.Equal("RefreshToken is required. (Parameter 'RefreshToken')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RefreshTokenAsync_ThrowsArgumentException_WhenClientIdIsNullOrEmpty(string clientId)
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            RefreshToken = "validRefreshToken",
            ClientId = clientId,
            ClientSecret = "validClientSecret"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authenticationClient.RefreshTokenAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("ClientId", exception.ParamName);
        Assert.Equal("ClientId must be provided. (Parameter 'ClientId')", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_ReturnsRefreshTokenResponse_WhenRequestIsValid()
    {
        // Arrange
        var refreshTokenRequest = new RefreshTokenRequest
        {
            RefreshToken = "validRefreshToken",
            ClientId = "validClientId",
            ClientSecret = "validClientSecret"
        };

        var mockResponse = new RefreshTokenResponse
        {
            AccessToken = "mockAccessToken",
            RefreshToken = "mockRefreshToken",
            BotId = "mockBotId",
            DuplicatedTemplateId = "mockDuplicatedTemplateId",
            Owner = new Owner
            {
                Workspace = true
            },
            WorkspaceIcon = "mockWorkspaceIcon",
            WorkspaceName = "mockWorkspaceName",
            WorkspaceId = "mockWorkspaceId"
        };

        _restClient.SetResponse(mockResponse);

        // Act
        var response = await _authenticationClient.RefreshTokenAsync(refreshTokenRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<RefreshTokenResponse>(response);
        Assert.Equal("mockAccessToken", response.AccessToken);
        Assert.Equal("mockRefreshToken", response.RefreshToken);
        Assert.Equal("mockBotId", response.BotId);
        Assert.Equal("mockDuplicatedTemplateId", response.DuplicatedTemplateId);
        Assert.NotNull(response.Owner);
        Assert.True(response.Owner.Workspace);
        Assert.Equal("mockWorkspaceIcon", response.WorkspaceIcon);
        Assert.Equal("mockWorkspaceName", response.WorkspaceName);
        Assert.Single(_restClient.Calls);
        Assert.Equal("POST", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.AuthenticationUrls.RefreshToken(), _restClient.LastCall.Uri);
        Assert.IsAssignableFrom<IRefreshTokenBodyParameters>(_restClient.LastCall.Body);
    }
}
