namespace CleanGraphQLApi.Application.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Enums;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Reviews.Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Review?>
{
    private readonly IReviewsRepository repository;

    public ReadByIdHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Review?> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadReviewById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Review);

        return result;
    }
}
