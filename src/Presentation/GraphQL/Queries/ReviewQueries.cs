namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Common.Paging;
using Application.Reviews.Entities;
using Extensions;
using HotChocolate.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Mediator;
using Reviews = Application.Reviews.Queries;

[QueryType]
public static partial class ReviewQueries
{
    /// <summary>
    /// Gets a review by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review.</param>
    [Authorize]
    public static async Task<Review?> GetReview(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Reviews.GetReviewById.GetReviewByIdQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    /// Lists reviews ordered by movie then author, one page at a time, optionally only those of one movie, by one author, or both.
    /// </summary>
    /// <param name="movieId">Only return reviews of this movie. Leave empty for every movie.</param>
    /// <param name="authorId">Only return reviews by this author. Leave empty for every author.</param>
    /// <param name="skip">The number of reviews to skip, 0 for the first page.</param>
    /// <param name="take">The largest number of reviews to return, from 1 to 50.</param>
    [UseOffsetPaging(CollectionSegmentName = "Reviews")]
    [Authorize]
    public static async Task<CollectionSegment<Review>> GetReviews(
        Guid? movieId,
        Guid? authorId,
        int? skip,
        int? take,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new Reviews.GetReviews.GetReviewsQuery
        {
            MovieId = movieId,
            AuthorId = authorId,
            Offset = skip ?? 0,
            Limit = take ?? Paging.DefaultLimit
        }, cancellationToken);

        return page.ToCollectionSegment();
    }
}
