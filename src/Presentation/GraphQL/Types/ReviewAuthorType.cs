namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using HotChocolate.Types;

[ObjectType<ReviewAuthor>]
public static partial class ReviewAuthorType
{
    static partial void Configure(IObjectTypeDescriptor<ReviewAuthor> descriptor)
    {
        _ = descriptor.Field(a => a.Id);
        _ = descriptor.Field(a => a.FirstName).Type<NonNullType<StringType>>();
        _ = descriptor.Field(a => a.LastName).Type<NonNullType<StringType>>();
    }
}
