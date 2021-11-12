namespace CleanGraphQL.Presentation.MovieReviews.Types.Objects;

using CleanGraphQL.Domain.Reviews.Entities;
using GraphQL.Types;

public sealed class ReviewObject : ObjectGraphType<Review>
{
    public ReviewObject()
    {
        this.Name = nameof(Review);
        this.Description = "A review in the collection";

        _ = this.Field(m => m.Id).Description("Identifier of the review");
        _ = this.Field(r => r.Stars).Description("Star rating out of five");
        _ = this.Field(
            name: "Movie",
            description: "The movie being reviewed",
            type: typeof(MovieObject),
            resolve: m => m.Source?.ReviewedMovie);
        _ = this.Field(
            name: "Author",
            description: "The author of the review",
            type: typeof(AuthorObject),
            resolve: m => m.Source?.ReviewAuthor);
    }
}
