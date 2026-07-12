namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IBasicAuthenticationParameters
{
    string ClientId { get; }
    string ClientSecret { get; }
}
