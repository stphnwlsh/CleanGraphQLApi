namespace CleanGraphQLApi.Presentation.GraphQL.Inputs;

/// <summary>
/// The new star rating of a review. The author and the movie of a review can't change.
/// </summary>
/// <param name="Id">The unique identifier of the review.</param>
/// <param name="Stars">The star rating, from 1 to 5.</param>
public record UpdateReviewInput(Guid Id, int Stars);
