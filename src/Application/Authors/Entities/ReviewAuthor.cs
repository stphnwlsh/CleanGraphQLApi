namespace CleanGraphQLApi.Application.Authors.Entities;

using Application.Common.Entities;

public record ReviewAuthor : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
