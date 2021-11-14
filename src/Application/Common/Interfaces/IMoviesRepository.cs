namespace CleanGraphQLApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanGraphQLApi.Domain.Movies.Entities;

public interface IMoviesRepository
{
    Task<List<Movie>> ReadAllMovies(CancellationToken cancellationToken);
    Task<Movie?> ReadMovieById(Guid id, CancellationToken cancellationToken);
    Task<bool> MovieExists(Guid id, CancellationToken cancellationToken);
}
