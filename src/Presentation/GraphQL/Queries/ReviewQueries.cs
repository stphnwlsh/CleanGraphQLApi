namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using MediatR;

using Entities = Application.Reviews.Entities;
using Queries = Application.Reviews.Queries;

// [GraphQLName("Reviews")]
// [GraphQLDescription("The queries used for all retireving information about the reviews in the collection.")]
public class ReviewQueries
{
    [GraphQLName("reviews")]
    [GraphQLDescription("Returns a list of all reviews in the collection.")]
    public async Task<List<Entities.Review>> GetReviewsAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new Queries.GetReviews.GetReviewsQuery(), cancellationToken);
    }
}
