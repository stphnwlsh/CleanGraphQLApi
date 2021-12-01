namespace CleanGraphQLApi.Application.Authors.ReadById;

using CleanGraphQLApi.Application.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Author>
{
    public Guid Id { get; set; }
}
