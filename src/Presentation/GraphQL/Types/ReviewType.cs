namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Reviews.Entities;
using global::GraphQL.Types;

public sealed class ReviewType : ObjectGraphType<Review>
{
    public ReviewType()
    {
        this.Name = nameof(Review);
        this.Description = "A review in the collection";

        _ = this.Field(m => m.Id).Description("Identifier of the review");
        _ = this.Field(r => r.Stars).Description("Star rating out of five");
        _ = this.Field(
            name: "Movie",
            description: "The movie being reviewed",
            type: typeof(MovieType),
            resolve: m => m.Source?.ReviewedMovie);
        _ = this.Field(
            name: "Author",
            description: "The author of the review",
            type: typeof(AuthorType),
            resolve: m => m.Source?.ReviewAuthor);
        _ = this.Field(m => m.DateCreated).Description("Date the review was created");
        _ = this.Field(m => m.DateModified).Description("Date the review was modified");
    }
}
