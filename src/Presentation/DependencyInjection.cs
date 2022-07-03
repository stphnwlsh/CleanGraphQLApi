namespace CleanGraphQLApi.Presentation;

using System.Diagnostics.CodeAnalysis;
using GraphQL.Authors.Queries;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        //_ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //_ = services.AddSingleton<IDocumentWriter, DocumentWriter>();

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //_ = services.AddSingleton<MovieReviewMutations>();
        //_ = services.AddSingleton<MovieReviewSchema>();

        return services;
    }
}
