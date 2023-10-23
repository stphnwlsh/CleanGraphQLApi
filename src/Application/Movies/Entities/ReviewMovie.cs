namespace CleanGraphQLApi.Application.Movies.Entities;

using Application.Common.Entities;

public record ReviewMovie : Entity
{
    public string Title { get; init; }
}
