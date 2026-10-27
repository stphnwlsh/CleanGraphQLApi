namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using global::GraphQL.Types;

public sealed class ReviewAuthorType : ObjectGraphType<ReviewAuthor>
{
    public ReviewAuthorType()
    {
        this.Name = nameof(ReviewAuthor);
        this.Description = "The author of a review";

        _ = this.Field(m => m.Id).Description("Identifier of the author");
        _ = this.Field(m => m.FirstName).Description("First name of the movie reviewer");
        _ = this.Field(m => m.LastName).Description("Last name of the movie reviewer");
    }
}
