namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;
using global::GraphQL.Types;
using MediatR;

public sealed class MovieType : ObjectGraphType<Movie>
{
    public MovieType(IMediator mediator)
    {
        this.Name = nameof(Movie);
        this.Description = "A movie in the collection";

        _ = this.Field(m => m.Id).Description("Identifier of the movie");
        _ = this.Field(m => m.Title).Description("Title of the movie");
        _ = this.Field(
            name: "Reviews",
            description: "Reviews of the movie",
            type: typeof(ListGraphType<ReviewType>),
            resolve: m => m.Source?.Reviews);
        _ = this.Field(m => m.DateCreated).Description("Date the movie was created");
        _ = this.Field(m => m.DateModified).Description("Date the movie was modified");
    }
}
