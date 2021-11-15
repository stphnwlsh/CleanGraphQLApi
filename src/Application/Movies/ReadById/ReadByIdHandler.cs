namespace CleanGraphQLApi.Application.Movies.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Movies.Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Movie?>
{
    private readonly IMoviesRepository repository;

    public ReadByIdHandler(IMoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Movie?> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadMovieById(request.Id, cancellationToken);
    }
}
