namespace CleanGraphQLApi.Application.Movies.ReadById;

using CleanGraphQLApi.Domain.Movies.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
