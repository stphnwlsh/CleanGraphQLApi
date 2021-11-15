namespace CleanGraphQLApi.Presentation.MovieReviews.Types.InputObjects;

using GraphQL.Types;

public sealed class ReviewInputType : InputObjectGraphType
{
    public ReviewInputType()
    {
        this.Name = "ReviewInput";
        this.Description = "A review of the movie";

        _ = this.Field<IdGraphType>("AuthorId", "Id of the Author of the review.");
        _ = this.Field<IdGraphType>("MovieId", "Id of the Movie being reviewed.");
        _ = this.Field<IntGraphType>("Stars", "Star rating out of five.");
    }
}
