namespace CleanGraphQLApi.Application.Reviews.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Reviews.Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Review>>
{
    private readonly IReviewsRepository repository;

    public ReadAllHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Review>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllReviews(cancellationToken);
    }
}
