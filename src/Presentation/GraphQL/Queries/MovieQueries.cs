namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Common.Paging;
using Application.Movies.Entities;
using Extensions;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Movies = Application.Movies.Queries;

[QueryType]
public static partial class MovieQueries
{
    /// <summary>
    /// Gets every movie, with the reviews written about it.
    /// </summary>
    public static async Task<List<Movie>> GetMovies(ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Movies.GetMovies.GetMoviesQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets a movie by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the movie.</param>
    public static async Task<Movie?> GetMovie(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Movies.GetMovieById.GetMovieByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Finds movies whose title contains a search term, one page at a time.
    /// </summary>
    /// <param name="query">The words to look for, ignoring case. Leave empty to list every movie.</param>
    [UsePaging(ConnectionName = "Movies")]
    public static async Task<Connection<Movie>> SearchMovies(
        string? query,
        int? first,
        string? after,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Movies.SearchMovies.SearchMoviesQuery
        {
            Query = query,
            Limit = first ?? Paging.DefaultLimit,
            Cursor = after
        }, cancellationToken);

        return page.ToConnection(after);
    }
}
