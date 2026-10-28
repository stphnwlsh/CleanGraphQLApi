namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Authors.Entities;
using Application.Common.Paging;
using Extensions;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Authors = Application.Authors.Queries;

[QueryType]
public static partial class AuthorQueries
{
    /// <summary>
    /// Gets every author, with the reviews they have written.
    /// </summary>
    public static async Task<List<Author>> GetAuthors(ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Authors.GetAuthors.GetAuthorsQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets an author by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    public static async Task<Author?> GetAuthor(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Authors.GetAuthorById.GetAuthorByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Finds authors whose first or last name contains a search term, one page at a time.
    /// </summary>
    /// <param name="query">The words to look for, ignoring case. Leave empty to list every author.</param>
    [UsePaging(ConnectionName = "Authors")]
    public static async Task<Connection<Author>> SearchAuthors(
        string? query,
        int? first,
        string? after,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Authors.SearchAuthors.SearchAuthorsQuery
        {
            Query = query,
            Limit = first ?? Paging.DefaultLimit,
            Cursor = after
        }, cancellationToken);

        return page.ToConnection(after);
    }
}
