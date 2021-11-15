namespace CleanGraphQLApi.Application.Reviews.ReadById;

using CleanGraphQLApi.Domain.Reviews.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Review>
{
    public Guid Id { get; set; }
}
