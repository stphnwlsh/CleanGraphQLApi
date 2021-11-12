namespace CleanGraphQL.Application.Reviews.ReadAll;

using CleanGraphQL.Domain.Reviews.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
