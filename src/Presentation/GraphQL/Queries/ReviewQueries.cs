namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Common.Paging;
using Application.Reviews.Entities;
using Extensions;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Reviews = Application.Reviews.Queries;

[QueryType]
public static partial class ReviewQueries
{
    /// <summary>
    /// Gets every review, with its movie and author.
    /// </summary>
    public static async Task<List<Review>> GetReviews(ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Reviews.GetReviews.GetReviewsQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets a review by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review.</param>
    public static async Task<Review?> GetReview(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Reviews.GetReviewById.GetReviewByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Finds reviews, optionally only those for one movie or by one author, one page at a time.
    /// </summary>
    /// <param name="movieId">Only return reviews of this movie. Leave empty for every movie.</param>
    /// <param name="authorId">Only return reviews by this author. Leave empty for every author.</param>
    [UsePaging(ConnectionName = "Reviews")]
    public static async Task<Connection<Review>> SearchReviews(
        Guid? movieId,
        Guid? authorId,
        int? first,
        string? after,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Reviews.SearchReviews.SearchReviewsQuery
        {
            MovieId = movieId,
            AuthorId = authorId,
            Limit = first ?? Paging.DefaultLimit,
            Cursor = after
        }, cancellationToken);

        return page.ToConnection(after);
    }
}
