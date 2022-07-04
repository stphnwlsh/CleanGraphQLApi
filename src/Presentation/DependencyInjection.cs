namespace CleanGraphQLApi.Presentation;

using System.Diagnostics.CodeAnalysis;
using CleanGraphQLApi.Presentation.GraphQL;
using CleanGraphQLApi.Presentation.GraphQL.Mutations;
using CleanGraphQLApi.Presentation.GraphQL.Queries;
using global::GraphQL.SystemTextJson;
using GraphQL;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        _ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        _ = services.AddSingleton<MovieReviewQueries>();
        _ = services.AddSingleton<MovieReviewMutations>();
        _ = services.AddSingleton<MovieReviewSchema>();

        return services;
    }
}
