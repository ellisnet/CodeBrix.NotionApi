using System;
using System.Text;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

internal static class HeaderHelpers
{
    public static string GetBasicAuthHeaderValue(IBasicAuthenticationParameters basicAuth)
    {
        if (basicAuth == null)
        {
            return null;
        }

        var basicAuthString = $"{basicAuth.ClientId}:{basicAuth.ClientSecret}";
        var basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(basicAuthString));

        return basicAuthHeaderValue;
    }
}
