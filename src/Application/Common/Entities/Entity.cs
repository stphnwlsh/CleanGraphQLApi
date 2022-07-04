namespace CleanGraphQLApi.Application.Common.Entities;

public record Entity
{
    public Guid Id { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }
}
