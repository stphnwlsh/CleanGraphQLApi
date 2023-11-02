namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;

public class MovieType : ObjectType<Movie>
{
    protected override void Configure(IObjectTypeDescriptor<Movie> descriptor)
    {
        this.Name = nameof(Movie);
        this.Description = "A movie in the collection";

        _ = descriptor.Field(m => m.Id).Description("Identifier of the movie");
        _ = descriptor.Field(m => m.Title).Description("Title of the movie");
        _ = descriptor
            .Field(m => m.Reviews)
            .Type<ListType<ReviewType>>()
            .Description("Reviews written about this movie");
        _ = descriptor.Field(m => m.DateCreated).Description("Date the movie was created");
        _ = descriptor.Field(m => m.DateModified).Description("Date the movie was modified");
    }
}
