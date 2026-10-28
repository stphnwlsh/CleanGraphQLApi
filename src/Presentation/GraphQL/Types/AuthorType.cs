namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using Application.Reviews.Entities;
using HotChocolate.Types;

[ObjectType<Author>]
public static partial class AuthorType
{
    static partial void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        _ = descriptor.Field(a => a.Id);
        _ = descriptor.Field(a => a.FirstName).Type<NonNullType<StringType>>();
        _ = descriptor.Field(a => a.LastName).Type<NonNullType<StringType>>();
        _ = descriptor.Field(a => a.Reviews).Type<ListType<NonNullType<ObjectType<Review>>>>();
    }
}
