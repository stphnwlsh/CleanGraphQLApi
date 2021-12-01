namespace CleanGraphQLApi.Presentation.MovieReviews.Types.Objects;

using CleanGraphQLApi.Application.Entities;
using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;

public sealed class AuthorObject : ObjectGraphType<Author>
{
    public AuthorObject(IDataLoaderContextAccessor dataLoaderAccessor, IMediator mediator)
    {
        this.Name = nameof(Author);
        this.Description = "An author in the collection";

        _ = this.Field(m => m.Id).Description("Identifier of the author");
        _ = this.Field(m => m.FirstName).Description("First name of the movie reviewer");
        _ = this.Field(m => m.LastName).Description("Last name of the movie reviewer");
        _ = this.Field(
            name: "Reviews",
            description: "This authors reviews",
            type: typeof(ListGraphType<ReviewObject>),
            resolve: m => m.Source?.Reviews);
        _ = this.Field(m => m.DateCreated).Description("Date the author was created");
        _ = this.Field(m => m.DateModified).Description("Date the author was modified");
    }
}
