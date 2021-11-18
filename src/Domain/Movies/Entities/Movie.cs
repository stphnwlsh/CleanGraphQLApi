namespace CleanGraphQLApi.Domain.Movies.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanGraphQLApi.Domain.Common.Entity;
using CleanGraphQLApi.Domain.Reviews.Entities;

public class Movie : Entity
{
    public string Title { get; set; }

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; init; }
}
