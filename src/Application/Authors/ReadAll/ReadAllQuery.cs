namespace CleanGraphQL.Application.Authors.ReadAll;

using CleanGraphQL.Domain.Authors.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
