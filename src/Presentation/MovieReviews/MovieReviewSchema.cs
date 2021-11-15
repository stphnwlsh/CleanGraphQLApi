namespace CleanGraphQLApi.Presentation.MovieReviews;

using System;
using GraphQL.Types;

public class MovieReviewSchema : Schema
{
    public MovieReviewSchema(MovieReviewQueries query, MovieReviewMutations mutation, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        this.Query = query;
        this.Mutation = mutation;
    }
}
