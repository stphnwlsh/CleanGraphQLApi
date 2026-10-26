namespace CleanGraphQLApi.Infrastructure.Databases.MoviesReviews.Mapping;

using Riok.Mapperly.Abstractions;
using AppAuthor = Application.Authors.Entities.Author;
using AppMovie = Application.Movies.Entities.Movie;
using AppReview = Application.Reviews.Entities.Review;
using InfraAuthor = Models.Author;
using InfraMovie = Models.Movie;
using InfraReview = Models.Review;

[Mapper(UseReferenceHandling = true)]
internal partial class MovieReviewsMapper
{
    public partial AppAuthor ToDomain(InfraAuthor author);

    public partial AppMovie ToDomain(InfraMovie movie);

    [MapperIgnoreSource(nameof(InfraReview.ReviewAuthorId))]
    [MapperIgnoreSource(nameof(InfraReview.ReviewedMovieId))]
    public partial AppReview ToDomain(InfraReview review);
}
