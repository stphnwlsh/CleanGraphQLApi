namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Common.Paging;
using Application.Movies.Entities;
using Extensions;
using HotChocolate.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Movies = Application.Movies.Queries;

[QueryType]
public static partial class MovieQueries
{
    /// <summary>
    /// Gets a movie by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the movie.</param>
    [Authorize]
    public static async Task<Movie?> GetMovie(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Movies.GetMovieById.GetMovieByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Lists movies ordered by title, one page at a time, optionally only those whose title contains a search term.
    /// </summary>
    /// <param name="query">The words to look for in the title, ignoring case. Leave empty to list every movie.</param>
    [UsePaging(ConnectionName = "Movies")]
    [Authorize]
    public static async Task<Connection<Movie>> GetMovies(
        string? query,
        int? first,
        string? after,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Movies.GetMovies.GetMoviesQuery
        {
            Query = query,
            Offset = after.ToOffset(),
            Limit = first ?? Paging.DefaultLimit
        }, cancellationToken);

        return page.ToConnection();
    }
}
