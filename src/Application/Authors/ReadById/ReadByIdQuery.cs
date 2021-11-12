namespace CleanGraphQL.Application.Authors.ReadById;

using CleanGraphQL.Domain.Authors.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Author>
{
    public Guid Id { get; set; }
}
