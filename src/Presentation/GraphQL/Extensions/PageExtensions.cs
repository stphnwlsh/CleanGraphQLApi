namespace CleanGraphQLApi.Presentation.GraphQL.Extensions;

using Application.Common.Paging;
using HotChocolate.Types.Pagination;

public static class PageExtensions
{
    /// <summary>
    /// Converts a page from the Application layer to a GraphQL collection segment.
    /// </summary>
    /// <param name="page">The page of results.</param>
    public static CollectionSegment<T> ToCollectionSegment<T>(this Page<T> page)
    {
        var pageInfo = new CollectionSegmentInfo(
            hasNextPage: page.Offset + page.Items.Count < page.TotalCount,
            hasPreviousPage: page.Offset > 0);

        return new CollectionSegment<T>(page.Items, pageInfo, page.TotalCount);
    }
}
