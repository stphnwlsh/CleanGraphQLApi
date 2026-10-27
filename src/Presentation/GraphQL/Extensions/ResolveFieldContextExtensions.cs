namespace CleanGraphQLApi.Presentation.GraphQL.Extensions;

using global::GraphQL;
using Mediator;

public static class ResolveFieldContextExtensions
{
    /// <summary>
    /// Resolves the <see cref="ISender"/> from the request scope so handlers get scoped dependencies.
    /// </summary>
    public static ISender Sender(this IResolveFieldContext context)
    {
        return context.RequestServices!.GetRequiredService<ISender>();
    }
}
