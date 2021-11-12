namespace CleanGraphQL.Application.Movies.ReadById;

using CleanGraphQL.Domain.Movies.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
