namespace CleanGraphQLApi.Application.Movies.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Movies.Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Movie>>
{
    private readonly IMoviesRepository repository;

    public ReadAllHandler(IMoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Movie>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllMovies(cancellationToken);
    }
}
