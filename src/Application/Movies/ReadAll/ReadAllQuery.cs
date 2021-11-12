namespace CleanGraphQL.Application.Movies.ReadAll;

using CleanGraphQL.Domain.Movies.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
