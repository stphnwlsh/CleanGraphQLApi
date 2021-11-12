namespace CleanGraphQL.Domain.Reviews.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanGraphQL.Domain.Authors.Entities;
using CleanGraphQL.Domain.Common.Entity;
using CleanGraphQL.Domain.Movies.Entities;

public class Review : Entity
{
    public int Stars { get; set; }

    [ForeignKey("ReviewedMovie")]
    public Guid ReviewedMovieId { get; set; }
    public Movie? ReviewedMovie { get; init; }

    [ForeignKey("ReviewAuthor")]
    public Guid ReviewAuthorId { get; set; }
    public Author? ReviewAuthor { get; init; }
}
