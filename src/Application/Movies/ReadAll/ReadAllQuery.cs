namespace CleanGraphQLApi.Application.Movies.ReadAll;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
