namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;

public sealed class ReviewAuthorType : ObjectType<ReviewAuthor>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewAuthor> descriptor)
    {
        _ = descriptor.Name(nameof(ReviewAuthor));
        _ = descriptor.Description("An author of a movie review");

        _ = descriptor.Field(m => m.Id).Description("Identifier of the author");
        _ = descriptor.Field(m => m.FirstName).Description("First name of the author");
        _ = descriptor.Field(m => m.LastName).Description("Last name of the author");
        _ = descriptor.Field(m => m.DateCreated).Description("Date the author was created");
        _ = descriptor.Field(m => m.DateModified).Description("Date the author was modified");
    }
}
