namespace CleanGraphQLApi.Application.Authors.ReadAll;

using CleanGraphQLApi.Domain.Authors.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
