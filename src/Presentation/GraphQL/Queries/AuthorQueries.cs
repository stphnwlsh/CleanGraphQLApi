namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using MediatR;

using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

// [GraphQLName("Authors")]
// [GraphQLDescription("The queries used for all retireving information about the authors in the collection.")]
public class AuthorQueries
{
    [GraphQLName("authors")]
    [GraphQLDescription("Returns a list of all authors in the collection.")]
    public async Task<List<Entities.Author>> GetAuthorsAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new Queries.GetAuthors.GetAuthorsQuery(), cancellationToken);
    }
}
