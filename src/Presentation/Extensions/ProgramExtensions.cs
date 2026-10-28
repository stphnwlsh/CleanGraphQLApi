namespace CleanGraphQLApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application;
using Application.Common.Paging;
using ChilliCream.Nitro.App;
using GraphQL.Errors;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[ExcludeFromCodeCoverage]
public static class ProgramExtensions
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
            .AddGraphQLServer()
            .AddPresentationTypes()
            .AddErrorFilter<ApplicationErrorFilter>()
            .AddMaxExecutionDepthRule(15)
            .ModifyServerOptions(options => options.Tool.ServeMode = ServeMode.Embedded)
            .ModifyPagingOptions(options =>
            {
                options.DefaultPageSize = Paging.DefaultLimit;
                options.MaxPageSize = Paging.MaxLimit;
                options.IncludeTotalCount = true;
                options.AllowBackwardPagination = false;
            });

        #endregion GraphQL

        #region Project Dependencies

        _ = builder.Services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        _ = builder.Services.AddInfrastructure();
        _ = builder.Services.AddApplication();

        #endregion Project Dependencies

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region GraphQL

        _ = app.MapGraphQL();

        #endregion GraphQL

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        return app;
    }
}
