namespace CleanGraphQL.Domain.Movies.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanGraphQL.Domain.Common.Entity;
using CleanGraphQL.Domain.Reviews.Entities;

public class Movie : Entity
{
    public string Title { get; set; } = string.Empty;

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; } = new List<Review>();
}
