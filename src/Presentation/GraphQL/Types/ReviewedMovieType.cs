namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;

public sealed class ReviewedMovieType : ObjectType<ReviewMovie>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewMovie> descriptor)
    {
        _ = descriptor.Name(nameof(ReviewMovie));
        _ = descriptor.Description("A moview being reviewed");

        _ = descriptor.Field(m => m.Id).Description("Identifier of the movie");
        _ = descriptor.Field(m => m.Title).Description("Title of the movie");
        _ = descriptor.Field(m => m.DateCreated).Description("Date the movie was created");
        _ = descriptor.Field(m => m.DateModified).Description("Date the movie was modified");
    }
}
