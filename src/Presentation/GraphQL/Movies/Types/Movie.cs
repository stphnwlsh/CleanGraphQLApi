namespace CleanGraphQLApi.Presentation.GraphQL.Movies.Types;

using Reviews.Types;

[GraphQLName("movie")]
[GraphQLDescription("An Movie to be Reviewed")]
public class Movie
{
    [GraphQLName("id")]
    [GraphQLDescription("Identifier of the Movie")]
    [GraphQLType(typeof(IdType))]
    public Guid Id { get; init; }

    [GraphQLName("title")]
    [GraphQLDescription("Title of the Movie")]
    [GraphQLType(typeof(StringType))]
    public string Title { get; init; }

    [GraphQLName("reviews")]
    [GraphQLDescription("Reviews of the Movie")]
    [GraphQLType(typeof(ListType))]
    public ICollection<Review> Reviews { get; init; }

    [GraphQLName("dateCreated")]
    [GraphQLDescription("Date the Movie was created")]
    [GraphQLType(typeof(DateTimeType))]
    public DateTime DateCreated { get; init; }

    [GraphQLName("dateModified")]
    [GraphQLDescription("Date the Movie was modified")]
    [GraphQLType(typeof(DateTimeType))]
    public DateTime DateModified { get; set; }
}
