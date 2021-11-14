namespace CleanGraphQLApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanGraphQLApi.Application;
using CleanGraphQLApi.Infrastructure;
using CleanGraphQLApi.Presentation.MovieReviews;
using GraphQL;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        #region Logging

        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            var assembly = Assembly.GetEntryAssembly();

            _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("Assembly Version", assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                    .Enrich.WithProperty("Assembly Informational Version", assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        });

        #endregion Logging

        #region GraphQL

        _ = builder.Services
            .AddGraphQL((options, provider) =>
            {
                // Load GraphQL Server configurations
                var graphQLOptions = builder.Configuration.GetSection("GraphQL").Get<GraphQLOptions>();

                options.ComplexityConfiguration = graphQLOptions.ComplexityConfiguration;
                options.EnableMetrics = graphQLOptions.EnableMetrics;

                var logger = provider.GetRequiredService<ILogger<Program>>();
                options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occurred", ctx.OriginalException.Message);
            })
            // Add required services for GraphQL request/response de/serialization
            .AddSystemTextJson() // For .NET Core 3+
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            //.AddWebSockets() // Add required services for web socket support
            .AddDataLoader() // Add required services for DataLoader support
            .AddGraphTypes(typeof(MovieReviewSchema)); // Add all IGraphType implementors in assembly which ChatSchema exists

        #endregion GraphQL

        #region Project Dependencies

        _ = builder.Services.AddInfrastructure();
        _ = builder.Services.AddApplication();
        _ = builder.Services.AddPresentation();

        #endregion Project Dependencies

        return builder;
    }
}
