namespace CleanGraphQLApi.Presentation.GraphQL.Reviews.Types;

using Authors.Types;
using Movies.Types;

[GraphQLName("review")]
[GraphQLDescription("An Review of a Movie")]
public record Review
{
    [GraphQLName("id")]
    [GraphQLDescription("Identifier of the Review")]
    [GraphQLType(typeof(IdType))]
    public Guid Id { get; init; }

    [GraphQLName("stars")]
    [GraphQLDescription("Star rating of the Movie out of 5")]
    [GraphQLType(typeof(IntType))]
    public int Stars { get; init; }

    [GraphQLName("author")]
    [GraphQLDescription("Author of the Review")]
    [GraphQLType(typeof(ObjectType))]
    public Author Author { get; init; }

    [GraphQLName("movie")]
    [GraphQLDescription("Movie being Reviewed")]
    [GraphQLType(typeof(ObjectType))]
    public Movie Movie { get; init; }

    [GraphQLName("dateCreated")]
    [GraphQLDescription("Date the Review was created")]
    [GraphQLType(typeof(DateTimeType))]
    public DateTime DateCreated { get; init; }

    [GraphQLName("dateModified")]
    [GraphQLDescription("Date the Review was modified")]
    [GraphQLType(typeof(DateTimeType))]
    public DateTime DateModified { get; set; }
}
