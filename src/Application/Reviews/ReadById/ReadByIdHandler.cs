namespace CleanGraphQL.Application.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQL.Application.Common.Interfaces;
using CleanGraphQL.Domain.Reviews.Entities;
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
        return await this.repository.ReadReviewById(request.Id, cancellationToken);
    }
}
