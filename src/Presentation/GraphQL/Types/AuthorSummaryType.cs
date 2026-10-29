namespace CleanGraphQLApi.Presentation.GraphQL.Types;

using Application.Authors.Entities;
using Application.Reviews.Entities;
using DataLoaders;
using HotChocolate;
using HotChocolate.Types;

[ObjectType<AuthorSummary>]
public static partial class AuthorSummaryType
{
    static partial void Configure(IObjectTypeDescriptor<AuthorSummary> descriptor)
    {
        _ = descriptor.Description("An author in a list, with how many reviews they have written and their average star rating. Their reviews are loaded for the whole page at once.");
        _ = descriptor.Field(a => a.Id);
        _ = descriptor.Field(a => a.FirstName).Type<NonNullType<StringType>>();
        _ = descriptor.Field(a => a.LastName).Type<NonNullType<StringType>>();
        _ = descriptor.Field(a => a.ReviewCount);
        _ = descriptor.Field(a => a.AverageStars);
    }

    /// <summary>
    /// The reviews the author has written. They're loaded for the whole page in one query.
    /// </summary>
    public static async Task<List<Review>> GetReviewsAsync(
        [Parent] AuthorSummary author,
        IReviewsByAuthorIdDataLoader reviews,
        CancellationToken cancellationToken)
    {
        // The DataLoader returns a list for every id, empty when there are no reviews, so a missing entry is a bug.
        return await reviews.LoadRequiredAsync(author.Id, cancellationToken);
    }
}
