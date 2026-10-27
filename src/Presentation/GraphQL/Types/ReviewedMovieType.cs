namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;
using global::GraphQL.Types;

public sealed class ReviewedMovieType : ObjectGraphType<ReviewedMovie>
{
    public ReviewedMovieType()
    {
        this.Name = nameof(ReviewedMovie);
        this.Description = "The movie a review is about";

        _ = this.Field(m => m.Id).Description("Identifier of the movie");
        _ = this.Field(m => m.Title).Description("Title of the movie");
    }
}
