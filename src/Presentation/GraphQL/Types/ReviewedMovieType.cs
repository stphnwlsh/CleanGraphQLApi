namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;
using HotChocolate.Types;

[ObjectType<ReviewedMovie>]
public static partial class ReviewedMovieType
{
    static partial void Configure(IObjectTypeDescriptor<ReviewedMovie> descriptor)
    {
        _ = descriptor.Field(m => m.Id);
        _ = descriptor.Field(m => m.Title).Type<NonNullType<StringType>>();
    }
}
