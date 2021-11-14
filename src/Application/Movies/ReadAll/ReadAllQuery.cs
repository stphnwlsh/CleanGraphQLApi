namespace CleanGraphQLApi.Application.Movies.ReadAll;

using CleanGraphQLApi.Domain.Movies.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
