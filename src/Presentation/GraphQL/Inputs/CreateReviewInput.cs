namespace CleanGraphQLApi.Presentation.GraphQL.Inputs;

/// <summary>
/// A review of a movie to create.
/// </summary>
/// <param name="AuthorId">The unique identifier of the author writing the review.</param>
/// <param name="MovieId">The unique identifier of the movie being reviewed.</param>
/// <param name="Stars">The star rating, from 1 to 5.</param>
public record CreateReviewInput(Guid AuthorId, Guid MovieId, int Stars);
