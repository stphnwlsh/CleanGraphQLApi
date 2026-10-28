namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;
using Application.Reviews.Entities;
using HotChocolate.Types;

[ObjectType<Movie>]
public static partial class MovieType
{
    static partial void Configure(IObjectTypeDescriptor<Movie> descriptor)
    {
        _ = descriptor.Field(m => m.Id);
        _ = descriptor.Field(m => m.Title).Type<NonNullType<StringType>>();
        _ = descriptor.Field(m => m.Reviews).Type<ListType<NonNullType<ObjectType<Review>>>>();
    }
}
