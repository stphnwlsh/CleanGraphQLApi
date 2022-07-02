namespace CleanGraphQLApi.Application.Authors.Entities;

using Application.Common.Entities;
using Application.Reviews.Entities;

public record Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
