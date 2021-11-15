namespace CleanGraphQLApi.Application.Common.Interfaces;

using System.Threading.Tasks;
using CleanGraphQLApi.Domain.Reviews.Entities;

public interface IReviewsRepository
{
    Task<Review> CreateReview(Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken);
    Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken);
    Task<List<Review>> ReadAllReviews(CancellationToken cancellationToken);
    Task<Review?> ReadReviewById(Guid id, CancellationToken cancellationToken);
    Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken);
}
