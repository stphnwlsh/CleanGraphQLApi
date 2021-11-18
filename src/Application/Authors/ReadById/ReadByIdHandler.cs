namespace CleanGraphQLApi.Application.Authors.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Enums;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Authors.Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Author?>
{
    private readonly IAuthorsRepository repository;

    public ReadByIdHandler(IAuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Author?> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadAuthorById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Author);

        return result;
    }
}
