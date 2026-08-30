using System.Threading.Tasks;
using CodeBrix.NotionApi;
using Xunit;

namespace CodeBrix.NotionApi.Tests.Integration; //was previously: Notion.IntegrationTests;

[Collection(NotionIntegrationCollection.Name)]
public class AuthenticationClientTests : IntegrationTestBase
{
    private readonly string _clientId = GetEnvironmentVariableRequired("NOTION_CLIENT_ID");
    private readonly string _clientSecret = GetEnvironmentVariableRequired("NOTION_CLIENT_SECRET");

    // An OAuth authorization code is single-use and short-lived, so it cannot be checked in: supply
    // a freshly minted one through NOTION_OAUTH_CODE when running these tests. (Upstream left its
    // own spent codes hard-coded here, which made the tests unrunnable for anyone else.)
    private readonly string _oauthCode = GetEnvironmentVariableRequired("NOTION_OAUTH_CODE");

    [Fact]
    public async Task Create_and_revoke_token()
    {
        // Arrange
        var createRequest = new CreateTokenRequest
        {
            Code = _oauthCode,
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            RedirectUri = "https://localhost:5001",
        };

        // Act
        var response = await Client.AuthenticationClient.CreateTokenAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.AccessToken);

        // revoke token
        await Client.AuthenticationClient.RevokeTokenAsync(new RevokeTokenRequest
        {
            Token = response.AccessToken,
            ClientId = _clientId,
            ClientSecret = _clientSecret
        }, cancellationToken: TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Introspect_token()
    {
        // Arrange
        var createRequest = new CreateTokenRequest
        {
            Code = _oauthCode,
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            RedirectUri = "https://localhost:5001",
        };

        // Act
        var response = await Client.AuthenticationClient.CreateTokenAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);

        // introspect token
        var introspectResponse = await Client.AuthenticationClient.IntrospectTokenAsync(new IntrospectTokenRequest
        {
            Token = response.AccessToken,
            ClientId = _clientId,
            ClientSecret = _clientSecret
        }, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(introspectResponse);
        Assert.True(introspectResponse.IsActive);

        // revoke token
        await Client.AuthenticationClient.RevokeTokenAsync(new RevokeTokenRequest
        {
            Token = response.AccessToken,
            ClientId = _clientId,
            ClientSecret = _clientSecret
        }, cancellationToken: TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Refresh_token()
    {
        // Arrange
        var createRequest = new CreateTokenRequest
        {
            Code = _oauthCode,
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            RedirectUri = "https://localhost:5001",
        };

        // Act
        var response = await Client.AuthenticationClient.CreateTokenAsync(createRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.AccessToken);

        // refresh token
        var refreshResponse = await Client.AuthenticationClient.RefreshTokenAsync(new RefreshTokenRequest
        {
            RefreshToken = response.RefreshToken,
            ClientId = _clientId,
            ClientSecret = _clientSecret
        }, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(refreshResponse);
        Assert.NotNull(refreshResponse.AccessToken);
        Assert.NotNull(refreshResponse.RefreshToken);
        Assert.NotEqual(response.AccessToken, refreshResponse.AccessToken);

        // revoke token
        await Client.AuthenticationClient.RevokeTokenAsync(new RevokeTokenRequest
        {
            Token = refreshResponse.AccessToken,
            ClientId = _clientId,
            ClientSecret = _clientSecret
        }, cancellationToken: TestContext.Current.CancellationToken);
    }
}
