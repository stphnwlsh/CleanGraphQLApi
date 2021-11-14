namespace CleanGraphQLApi.Application.Reviews.ReadAll;

using CleanGraphQLApi.Domain.Reviews.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
