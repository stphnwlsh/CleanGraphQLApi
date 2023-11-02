namespace CleanGraphQLApi.Presentation.GraphQL.InputTypes;

public class UpdateReview
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public Guid MovieId { get; set; }

    public int Stars { get; set; }
}
