namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using MediatR;

using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

// [GraphQLName("Movies")]
// [GraphQLDescription("The queries used for all retreiving information about movies in the collection.")]
public class MovieQueries
{
    [GraphQLName("movies")]
    [GraphQLDescription("Returns a list of all movies in the collection.")]
    public async Task<List<Entities.Movie>> GetMoviesAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new Queries.GetMovies.GetMoviesQuery(), cancellationToken);
    }
}
