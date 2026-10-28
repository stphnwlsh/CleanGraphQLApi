namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Reviews.Entities;
using HotChocolate.Types;

[ObjectType<Review>]
public static partial class ReviewType
{
    static partial void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        _ = descriptor.Field(r => r.Id);
        _ = descriptor.Field(r => r.Stars);
        _ = descriptor.Field(r => r.ReviewedMovie).Name("movie");
        _ = descriptor.Field(r => r.ReviewAuthor).Name("author");
    }
}
