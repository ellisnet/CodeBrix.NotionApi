using System;
using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class AuthenticationClientTests
{
    private readonly RecordingRestClient _restClient = new();
    private readonly AuthenticationClient _authenticationClient;

    public AuthenticationClientTests()
    {
        _authenticationClient = new AuthenticationClient(_restClient);
    }

    [Fact]
    public async Task IntrospectTokenAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticationClient.IntrospectTokenAsync(null, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("introspectTokenRequest", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'introspectTokenRequest')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task IntrospectTokenAsync_ThrowsArgumentException_WhenTokenIsNullOrEmpty(string token)
    {
        // Arrange
        var request = new IntrospectTokenRequest
        {
            Token = token,
            ClientId = "validClientId",
            ClientSecret = "validClientSecret"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authenticationClient.IntrospectTokenAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("Token", exception.ParamName);
        Assert.Equal("Token must be provided. (Parameter 'Token')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task IntrospectTokenAsync_ThrowsArgumentException_WhenClientIdIsNullOrEmpty(string clientId)
    {
        // Arrange
        var request = new IntrospectTokenRequest
        {
            Token = "validToken",
            ClientId = clientId,
            ClientSecret = "validClientSecret"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authenticationClient.IntrospectTokenAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("ClientId", exception.ParamName);
        Assert.Equal("ClientId must be provided. (Parameter 'ClientId')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task IntrospectTokenAsync_ThrowsArgumentException_WhenClientSecretIsNullOrEmpty(string clientSecret)
    {
        // Arrange
        var request = new IntrospectTokenRequest
        {
            Token = "validToken",
            ClientId = "validClientId",
            ClientSecret = clientSecret
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authenticationClient.IntrospectTokenAsync(request, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal("ClientSecret", exception.ParamName);
        Assert.Equal("ClientSecret must be provided. (Parameter 'ClientSecret')", exception.Message);
    }

    [Fact]
    public async Task IntrospectTokenAsync_CallsPostAsync_WithCorrectParameters()
    {
        // Arrange
        var introspectTokenRequest = new IntrospectTokenRequest
        {
            Token = "validToken",
            ClientId = "validClientId",
            ClientSecret = "validClientSecret"
        };

        var expectedResponse = new IntrospectTokenResponse
        {
            IsActive = true,
            Scope = "read write",
            Iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        _restClient.SetResponse(expectedResponse);

        // Act
        var response = await _authenticationClient.IntrospectTokenAsync(introspectTokenRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedResponse.IsActive, response.IsActive);
        Assert.Equal(expectedResponse.Scope, response.Scope);
        Assert.Equal(expectedResponse.Iat, response.Iat);
        Assert.Single(_restClient.Calls);
        Assert.Equal("POST", _restClient.LastCall.Method);
        Assert.Equal(ApiEndpoints.AuthenticationUrls.IntrospectToken(), _restClient.LastCall.Uri);
        Assert.IsAssignableFrom<IIntrospectTokenBodyParameters>(_restClient.LastCall.Body);
    }
}
