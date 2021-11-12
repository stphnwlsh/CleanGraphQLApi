namespace CleanGraphQL.Domain.Authors.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanGraphQL.Domain.Common.Entity;
using CleanGraphQL.Domain.Reviews.Entities;

public class Author : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; } = new List<Review>();
}
