namespace CleanGraphQLApi.Presentation.MovieReviews.Types.InputObjects;

using GraphQL.Types;

public sealed class CreateReviewInputType : InputObjectGraphType
{
    public CreateReviewInputType()
    {
        this.Name = "CreateReviewInput";
        this.Description = "A review of a movie to be created";

        _ = this.Field<IdGraphType>("AuthorId", "Id of the Author of the review.");
        _ = this.Field<IdGraphType>("MovieId", "Id of the Movie being reviewed.");
        _ = this.Field<IntGraphType>("Stars", "Star rating out of five.");
    }
}
