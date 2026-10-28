namespace CleanGraphQLApi.Presentation.GraphQL.Mutations;

using Application.Reviews.Entities;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Inputs;
using Mediator;
using Commands = Application.Reviews.Commands;

[MutationType]
public static partial class ReviewMutations
{
    /// <summary>
    /// Creates a review of a movie and returns it.
    /// </summary>
    /// <param name="input">The review to create.</param>
    [Authorize]
    public static async Task<Review?> CreateReview(CreateReviewInput input, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Commands.CreateReview.CreateReviewCommand
        {
            AuthorId = input.AuthorId,
            MovieId = input.MovieId,
            Stars = input.Stars
        }, cancellationToken);
    }

    /// <summary>
    /// Updates the star rating of a review. Returns true when the review was updated.
    /// </summary>
    /// <param name="input">The review and its new star rating.</param>
    [Authorize]
    public static async Task<bool?> UpdateReview(UpdateReviewInput input, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Commands.UpdateReview.UpdateReviewCommand
        {
            Id = input.Id,
            Stars = input.Stars
        }, cancellationToken);
    }

    /// <summary>
    /// Deletes a review. Returns true when the review was deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the review.</param>
    [Authorize]
    public static async Task<bool?> DeleteReview(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(new Commands.DeleteReview.DeleteReviewCommand { Id = id }, cancellationToken);
    }
}
