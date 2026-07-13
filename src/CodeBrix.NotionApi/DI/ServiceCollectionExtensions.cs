using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBrix.NotionApi; //was previously: Microsoft.Extensions.DependencyInjection;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="INotionClientFactory"/> as a singleton, backed by an
    /// <see cref="System.Net.Http.IHttpClientFactory"/>-managed named <see cref="System.Net.Http.HttpClient"/>.
    /// This is the recommended registration: inject <see cref="INotionClientFactory"/> and call
    /// <see cref="INotionClientFactory.Create"/> per unit of work (wrapping the result in a
    /// <c>using</c>). Every client the factory creates draws its <see cref="System.Net.Http.HttpClient"/>
    /// from the factory-managed pool — resolved per request, so handler lifetimes and DNS refresh are
    /// handled correctly for long-running applications.
    /// </summary>
    /// <param name="services">The service collection to add the registration to.</param>
    /// <param name="configureClient">
    /// Optional configuration for the named <see cref="System.Net.Http.HttpClient"/> (for example to set
    /// a non-default <c>BaseAddress</c> or timeouts). The base address defaults to the Notion API root.
    /// </param>
    /// <returns>The same <paramref name="services"/> instance, for chaining.</returns>
    public static IServiceCollection AddNotionClientFactory(
        this IServiceCollection services,
        Action<HttpClient> configureClient = null)
    {
        var builder = services.AddHttpClient(Constants.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(Constants.BaseUrl);
            configureClient?.Invoke(client);
        });

        builder.AddHttpMessageHandler(() => new LoggingHandler());

        services.AddSingleton<INotionClientFactory>(sp =>
            new NotionClientFactory(sp.GetRequiredService<IHttpClientFactory>()));

        return services;
    }

    /// <summary>
    /// Registers <see cref="INotionClient"/> as a singleton and configures the underlying
    /// <see cref="System.Net.Http.HttpClient"/> through <see cref="System.Net.Http.IHttpClientFactory"/>,
    /// which correctly manages handler lifetimes and DNS refresh for long-running applications.
    /// <para>
    /// Use this when a single, app-wide <see cref="INotionClient"/> is all you need. If you instead want
    /// to create (and dispose) clients per unit of work, register
    /// <see cref="AddNotionClientFactory"/> and inject <see cref="INotionClientFactory"/>.
    /// </para>
    /// </summary>
    public static IServiceCollection AddNotionClient(
        this IServiceCollection services,
        Action<ClientOptions> configureOptions)
    {
        var clientOptions = new ClientOptions();
        configureOptions?.Invoke(clientOptions);

        // Register a named HttpClient managed by IHttpClientFactory.
        // IHttpClientFactory rotates the underlying HttpClientHandler periodically,
        // preventing stale DNS and socket exhaustion while keeping HttpClient reuse safe.
        var builder = services.AddHttpClient(Constants.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(clientOptions.BaseUrl ?? Constants.BaseUrl);
        });

        builder.AddHttpMessageHandler(() => new LoggingHandler());

        services.AddSingleton<INotionClient>(sp =>
        {
            // Hand the IHttpClientFactory itself to the client (via NotionClientFactory) rather than a
            // single pre-created HttpClient. The client then resolves CreateClient() per request, so the
            // handler actually rotates — a pre-created, cached client would never pick up rotation.
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();

            // Pass RetryPolicy through; RestClient applies it in its own SendAsync loop
            // so it works regardless of which HttpClient is in use.
            var options = new ClientOptions
            {
                AuthToken = clientOptions.AuthToken,
                BaseUrl = clientOptions.BaseUrl,
                NotionVersion = clientOptions.NotionVersion,
                RetryPolicy = clientOptions.RetryPolicy
            };

            return new NotionClientFactory(httpClientFactory).Create(options);
        });

        return services;
    }
}
