namespace CleanGraphQLApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application;
using Application.Common.Paging;
using Authentication;
using ChilliCream.Nitro.App;
using GraphQL.Errors;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        #region Authentication

        var authentication = AuthenticationSettings.FromConfiguration(builder.Configuration);

        if (authentication.Enabled)
        {
            _ = builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authentication.Authority;
                    options.Audience = authentication.Audience;
                    options.MapInboundClaims = false;
                });
            _ = builder.Services.AddAuthorization();
        }
        else
        {
            // Fields always ask for authorization, so without an authority every request is let through.
            _ = builder.Services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build());
        }

        #endregion Authentication

        #region GraphQL

        _ = builder.Services
            .AddGraphQLServer()
            .AddPresentationTypes()
            .AddAuthorization()
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
        _ = builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("MovieReviews"));
        _ = builder.Services.AddApplication();
        _ = builder.Services.AddHealthChecks().AddInfrastructureHealthChecks("ready");

        #endregion Project Dependencies

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Database

        _ = app.Services.InitialiseInfrastructure(app.Configuration.GetValue<bool>("Database:SeedSampleData"));

        #endregion Database

        #region Security

        // HSTS tells browsers to only use HTTPS from then on, so it's left off for local runs.
        if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("local"))
        {
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        #endregion Security

        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Authentication

        if (AuthenticationSettings.FromConfiguration(app.Configuration).Enabled)
        {
            _ = app.UseAuthentication();
        }

        _ = app.UseAuthorization();

        #endregion Authentication

        #region Health

        // Liveness runs no checks, so it only fails when the process can't answer. Readiness checks the database.
        _ = app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false }).AllowAnonymous();
        _ = app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") }).AllowAnonymous();

        #endregion Health

        #region GraphQL

        _ = app.MapGraphQL();

        #endregion GraphQL

        return app;
    }
}
