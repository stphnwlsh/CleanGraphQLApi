namespace CleanGraphQLApi.Application.Reviews.Create;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Reviews.Entities;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, Review>
{
    private readonly IAuthorsRepository authorsRepository;
    private readonly IMoviesRepository moviesRepository;
    private readonly IReviewsRepository reviewsRepository;

    public CreateHandler(IAuthorsRepository authorsRepository, IMoviesRepository moviesRepository, IReviewsRepository reviewsRepository)
    {
        this.authorsRepository = authorsRepository;
        this.moviesRepository = moviesRepository;
        this.reviewsRepository = reviewsRepository;
    }

    public async Task<Review> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!await this.authorsRepository.AuthorExists(request.AuthorId, cancellationToken))
        {
            throw new NotFoundException(Common.Enums.EntityType.Author, "An author with the supplied id was not found.");
        }

        if (!await this.moviesRepository.MovieExists(request.MovieId, cancellationToken))
        {
            throw new NotFoundException(Common.Enums.EntityType.Movie, "A movie with the supplied id was not found.");
        }

        return await this.reviewsRepository.CreateReview(request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
