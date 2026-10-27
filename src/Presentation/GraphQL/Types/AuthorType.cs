namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using global::GraphQL.Types;

public sealed class AuthorType : ObjectGraphType<Author>
{
    public AuthorType()
    {
        this.Name = nameof(Author);
        this.Description = "An author in the collection";

        _ = this.Field(m => m.Id).Description("Identifier of the author");
        _ = this.Field(m => m.FirstName).Description("First name of the movie reviewer");
        _ = this.Field(m => m.LastName).Description("Last name of the movie reviewer");
        _ = this.Field(
            name: "Reviews",
            description: "This authors reviews",
            type: typeof(ListGraphType<ReviewType>),
            resolve: m => m.Source?.Reviews);
    }
}
