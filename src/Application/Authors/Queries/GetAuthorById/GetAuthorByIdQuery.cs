namespace CleanGraphQLApi.Application.Authors.Queries.GetAuthorById;

using System.ComponentModel.DataAnnotations;
using Entities;
using MediatR;

public class GetAuthorByIdQuery : IRequest<Author>
{
    [Required]
    public Guid Id { get; init; }
}
