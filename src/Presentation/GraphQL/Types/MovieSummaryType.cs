namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Movies.Entities;
using Application.Reviews.Entities;
using DataLoaders;
using HotChocolate;
using HotChocolate.Types;

[ObjectType<MovieSummary>]
public static partial class MovieSummaryType
{
    static partial void Configure(IObjectTypeDescriptor<MovieSummary> descriptor)
    {
        _ = descriptor.Description("A movie in a list, with how many reviews it has and their average star rating. Its reviews are loaded for the whole page at once.");
        _ = descriptor.Field(m => m.Id);
        _ = descriptor.Field(m => m.Title).Type<NonNullType<StringType>>();
        _ = descriptor.Field(m => m.ReviewCount);
        _ = descriptor.Field(m => m.AverageStars);
    }

    /// <summary>
    /// The reviews written about the movie. They're loaded for the whole page in one query.
    /// </summary>
    public static async Task<List<Review>> GetReviewsAsync(
        [Parent] MovieSummary movie,
        IReviewsByMovieIdDataLoader reviews,
        CancellationToken cancellationToken)
    {
        return await reviews.LoadAsync(movie.Id, cancellationToken) ?? [];
    }
}
