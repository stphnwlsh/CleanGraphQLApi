namespace CleanGraphQLApi.Application.Reviews.Delete;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand, bool>
{
    private readonly IReviewsRepository repository;

    public DeleteHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await this.repository.ReviewExists(request.Id, cancellationToken))
        {
            throw new NotFoundException(Common.Enums.EntityType.Review, "A review with the supplied id was not found.");
        }

        return await this.repository.DeleteReview(request.Id, cancellationToken);
    }
}
