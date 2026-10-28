namespace CleanGraphQLApi.Presentation.GraphQL.Inputs;

/// <summary>
/// A review of a movie to update.
/// </summary>
/// <param name="Id">The unique identifier of the review.</param>
/// <param name="AuthorId">The unique identifier of the author who wrote the review.</param>
/// <param name="MovieId">The unique identifier of the movie being reviewed.</param>
/// <param name="Stars">The star rating, from 1 to 5.</param>
public record UpdateReviewInput(Guid Id, Guid AuthorId, Guid MovieId, int Stars);
