namespace CleanGraphQL.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using CleanGraphQL.Presentation.MovieReviews;
using Microsoft.AspNetCore.Builder;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region GraphQL

        _ = app.UseGraphQL<MovieReviewSchema>();
        _ = app.UseGraphQLPlayground();

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
