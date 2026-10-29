namespace CleanGraphQLApi.Presentation.GraphQL.DataLoaders;

using Application.Common.Paging;
using Application.Reviews.Entities;
using GreenDonut;
using Mediator;
using Reviews = Application.Reviews.Queries;

/// <summary>
/// Batches the reviews asked for by every movie or author in a response into one query, instead of one per item.
/// </summary>
public static class ReviewDataLoaders
{
    /// <summary>
    /// Gets the reviews of each movie, in batches no larger than the Application layer accepts.
    /// </summary>
    /// <param name="movieIds">The movies whose reviews were asked for.</param>
    /// <param name="sender">Sends the query to the Application layer.</param>
    /// <param name="cancellationToken">Cancels the batch.</param>
    [DataLoader]
    public static async Task<Dictionary<Guid, List<Review>>> GetReviewsByMovieIdAsync(
        IReadOnlyList<Guid> movieIds,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var reviews = new Dictionary<Guid, List<Review>>();

        foreach (var batch in movieIds.Chunk(Paging.MaxLimit))
        {
            foreach (var (id, items) in await sender.Send(new Reviews.GetReviewsForMovies.GetReviewsForMoviesQuery { MovieIds = batch }, cancellationToken))
            {
                reviews[id] = items;
            }
        }

        return reviews;
    }

    /// <summary>
    /// Gets the reviews written by each author, in batches no larger than the Application layer accepts.
    /// </summary>
    /// <param name="authorIds">The authors whose reviews were asked for.</param>
    /// <param name="sender">Sends the query to the Application layer.</param>
    /// <param name="cancellationToken">Cancels the batch.</param>
    [DataLoader]
    public static async Task<Dictionary<Guid, List<Review>>> GetReviewsByAuthorIdAsync(
        IReadOnlyList<Guid> authorIds,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var reviews = new Dictionary<Guid, List<Review>>();

        foreach (var batch in authorIds.Chunk(Paging.MaxLimit))
        {
            foreach (var (id, items) in await sender.Send(new Reviews.GetReviewsForAuthors.GetReviewsForAuthorsQuery { AuthorIds = batch }, cancellationToken))
            {
                reviews[id] = items;
            }
        }

        return reviews;
    }
}
