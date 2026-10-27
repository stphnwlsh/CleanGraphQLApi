namespace CleanGraphQLApi.Application.Authors.Entities;

using Application.Reviews.Entities;

public record Author(Guid Id, string FirstName, string LastName, ICollection<Review> Reviews = null);
