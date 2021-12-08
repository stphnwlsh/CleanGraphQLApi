namespace CleanGraphQLApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanGraphQLApi.Application;
using CleanGraphQLApi.Infrastructure;
using CleanGraphQLApi.Presentation.MovieReviews;
using GraphQL;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        #region Settings

        _ = builder.Host.ConfigureAppConfiguration((hostingContext, config) => config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName ?? "local"}.json", true, true)
            .AddEnvironmentVariables());

        #endregion Settings

        #region Logging

        var useLogging = builder.Configuration.GetValue<bool>("UseLogging");
        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            if (useLogging)
            {
                var assembly = Assembly.GetEntryAssembly();

                _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("Assembly Version", assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                    .Enrich.WithProperty("Assembly Informational Version", assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
            }
        });

        #endregion Logging

        #region Telemetry

        var useTelemetry = builder.Configuration.GetValue<bool>("UseTelemetry");
        if (useTelemetry)
        {
            _ = builder.Services.AddOpenTelemetryTracing((services) => services
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Configuration.GetValue<string>("Zipkin:ServiceName")))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddZipkinExporter());

            _ = builder.Services.Configure<ZipkinExporterOptions>(builder.Configuration.GetSection("Zipkin"));
        }

        #endregion Telemetry






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
