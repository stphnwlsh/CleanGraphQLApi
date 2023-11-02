namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;

public sealed class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        _ = descriptor.Name(nameof(Author));
        _ = descriptor.Description("An author in the collection");

        _ = descriptor.Field(a => a.Id).Description("Identifier of the author");
        _ = descriptor.Field(a => a.FirstName).Description("First name of the author");
        _ = descriptor.Field(a => a.LastName).Description("Last name of the author");
        _ = descriptor
            .Field(a => a.Reviews)
            .Type<ListType<ReviewType>>()
            .Description("Reviews written by this author");
        _ = descriptor.Field(a => a.DateCreated).Description("Date the author was created");
        _ = descriptor.Field(a => a.DateModified).Description("Date the author was modified");
    }
}
