namespace CleanGraphQLApi.Infrastructure.Persistance.InMemory.MovieReviews;

using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Domain.Authors.Entities;
using CleanGraphQLApi.Domain.Movies.Entities;
using CleanGraphQLApi.Domain.Reviews.Entities;
using Microsoft.EntityFrameworkCore;

internal class MovieReviewsRepository : IAuthorsRepository, IMoviesRepository, IReviewsRepository
{
    private readonly MovieReviewsDbContext context;

    public MovieReviewsRepository(MovieReviewsDbContext context)
    {
        this.context = context;

        if (this.context != null)
        {
            _ = this.context.Database.EnsureDeleted();
            _ = this.context.Database.EnsureCreated();
            _ = this.context.AddData();
        }
    }

    #region Authors

    public virtual async Task<List<Author>> ReadAllAuthors(CancellationToken cancellationToken)
    {
        return await this.context.Authors.Include(a => a.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<Author?> ReadAuthorById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.Where(r => r.Id == id).Include(m => m.Reviews).ThenInclude(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> AuthorExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Authors

    #region Movies

    public virtual async Task<List<Movie>> ReadAllMovies(CancellationToken cancellationToken)
    {
        return await this.context.Movies.Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<Movie?> ReadMovieById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.Where(r => r.Id == id).Include(m => m.Reviews).ThenInclude(r => r.ReviewAuthor).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> MovieExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Movies.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    }

    #endregion Movies

    #region Reviews

    public async Task<Review> CreateReview(Guid authorId, Guid movieId, int stars, CancellationToken cancellationToken)
    {
        var review = new Review { ReviewAuthorId = authorId, ReviewedMovieId = movieId, Stars = stars };

        var id = this.context.Add(review).Entity.Id;

        _ = await this.context.SaveChangesAsync(cancellationToken);

        return await this.context.Reviews.Where(r => r.Id == id).Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().FirstAsync(cancellationToken);
    }

    public async Task<bool> DeleteReview(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _ = this.context.Remove(this.context.Reviews.Single(r => r.Id == id));
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<List<Review>> ReadAllReviews(CancellationToken cancellationToken)
    {
        return await this.context.Reviews.Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Review?> ReadReviewById(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.Where(r => r.Id == id).Include(r => r.ReviewAuthor).Include(r => r.ReviewedMovie).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ReviewExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Reviews.AnyAsync(r => r.Id == id, cancellationToken);
    }

    #endregion Reviews
}
