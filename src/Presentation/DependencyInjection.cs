namespace CleanGraphQLApi.Presentation;

using System.Diagnostics.CodeAnalysis;
using CleanGraphQLApi.Presentation.MovieReviews;
using GraphQL;
using GraphQL.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        _ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        _ = services.AddSingleton<IDocumentWriter, DocumentWriter>();

        _ = services.AddSingleton<MovieReviewQueries>();
        _ = services.AddSingleton<MovieReviewMutations>();
        _ = services.AddSingleton<MovieReviewSchema>();

        return services;
    }
}
