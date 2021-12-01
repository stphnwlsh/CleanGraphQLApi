namespace CleanGraphQLApi.Application.Reviews.ReadAll;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
