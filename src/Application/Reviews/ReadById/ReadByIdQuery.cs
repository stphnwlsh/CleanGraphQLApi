namespace CleanGraphQL.Application.Reviews.ReadById;

using CleanGraphQL.Domain.Reviews.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Review>
{
    public Guid Id { get; set; }
}
