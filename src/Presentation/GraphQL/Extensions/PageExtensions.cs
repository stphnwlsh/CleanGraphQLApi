namespace CleanGraphQLApi.Presentation.GraphQL.Extensions;

using Application.Common.Paging;
using HotChocolate.Types.Pagination;

public static class PageExtensions
{
    /// <summary>
    /// Converts a page from the Application layer to a GraphQL connection. Each edge's cursor fetches the results after it.
    /// </summary>
    /// <param name="page">The page of results.</param>
    /// <param name="after">The cursor the page was fetched with.</param>
    public static Connection<T> ToConnection<T>(this Page<T> page, string? after)
    {
        _ = Paging.TryDecode(after, out var offset);

        var edges = page.Items
            .Select((item, index) => new Edge<T>(item, Paging.Encode(offset + index + 1)))
            .ToList();

        var pageInfo = new ConnectionPageInfo(
            hasNextPage: page.NextCursor is not null,
            hasPreviousPage: offset > 0,
            startCursor: edges.FirstOrDefault()?.Cursor,
            endCursor: edges.LastOrDefault()?.Cursor);

        return new Connection<T>(edges, pageInfo, page.TotalCount);
    }
}
