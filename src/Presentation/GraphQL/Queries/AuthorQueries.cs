namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Authors.Entities;
using Application.Common.Paging;
using Extensions;
using HotChocolate.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Authors = Application.Authors.Queries;

[QueryType]
public static partial class AuthorQueries
{
    /// <summary>
    /// Gets an author by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    [Authorize]
    public static async Task<Author?> GetAuthor(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Authors.GetAuthorById.GetAuthorByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Lists authors ordered by last name then first name, one page at a time, optionally only those whose name contains a search term.
    /// </summary>
    /// <param name="query">The words to look for in the name, ignoring case. Leave empty to list every author.</param>
    /// <param name="skip">The number of authors to skip, 0 for the first page.</param>
    /// <param name="take">The largest number of authors to return, from 1 to 50.</param>
    [UseOffsetPaging(CollectionSegmentName = "Authors")]
    [Authorize]
    public static async Task<CollectionSegment<Author>> GetAuthors(
        string? query,
        int? skip,
        int? take,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Authors.GetAuthors.GetAuthorsQuery
        {
            Query = query,
            Offset = skip ?? 0,
            Limit = take ?? Paging.DefaultLimit
        }, cancellationToken);

        return page.ToCollectionSegment();
    }
}
