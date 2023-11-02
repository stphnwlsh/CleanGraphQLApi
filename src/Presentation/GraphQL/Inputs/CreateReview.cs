namespace CleanGraphQLApi.Presentation.GraphQL.InputTypes;

public class CreateReview
{
    public Guid AuthorId { get; set; }

    public Guid MovieId { get; set; }

    public int Stars { get; set; }
}
