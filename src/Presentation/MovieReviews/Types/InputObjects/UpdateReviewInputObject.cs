namespace CleanGraphQLApi.Presentation.MovieReviews.Types.InputObjects;

using GraphQL.Types;

public sealed class UpdateReviewInputType : InputObjectGraphType
{
    public UpdateReviewInputType()
    {
        this.Name = "UpdateReviewInput";
        this.Description = "A review of a movie to be updated";

        _ = this.Field<IdGraphType>("Id", "Id of the review.");
        _ = this.Field<IdGraphType>("AuthorId", "Id of the Author of the review.");
        _ = this.Field<IdGraphType>("MovieId", "Id of the Movie being reviewed.");
        _ = this.Field<IntGraphType>("Stars", "Star rating out of five.");
    }
}
