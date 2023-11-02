namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using Application.Movies.Entities;
using Application.Reviews.Entities;

public sealed class ReviewType : ObjectType<Review>
{
    protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        _ = descriptor.Name(nameof(Review));
        _ = descriptor.Description("A review in the collection");

        _ = descriptor.Field(r => r.Id).Description("Identifier of the review");
        _ = descriptor.Field(r => r.Stars).Description("Star rating out of five");
        _ = descriptor
            .Name(nameof(Author))
            .Field(r => r.ReviewAuthor)
            .Type<ReviewAuthorType>()
            .Description("Author who wrote this movie review");
        _ = descriptor
            .Name(nameof(Movie))
            .Field(r => r.ReviewedMovie)
            .Type<ReviewAuthorType>()
            .Description("Movie that is being reviewed");
        _ = descriptor.Field(m => m.DateCreated).Description("Date the review was created");
        _ = descriptor.Field(m => m.DateModified).Description("Date the review was modified");
    }
}
