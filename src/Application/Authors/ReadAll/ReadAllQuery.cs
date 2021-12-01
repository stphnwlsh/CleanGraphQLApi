namespace CleanGraphQLApi.Application.Authors.ReadAll;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
