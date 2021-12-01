namespace CleanGraphQLApi.Application.Movies.ReadById;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
