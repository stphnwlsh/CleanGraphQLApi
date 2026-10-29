namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;
using Reviews.Query;

public partial class Data
{
    [JsonPropertyName("author")]
    public Authors.Author Author { get; set; } = null!;

    [JsonPropertyName("authors")]
    public Connection<Authors.Author> Authors { get; set; } = null!;

    [JsonPropertyName("movie")]
    public Movies.Movie Movie { get; set; } = null!;

    [JsonPropertyName("movies")]
    public Connection<Movies.Movie> Movies { get; set; } = null!;

    [JsonPropertyName("review")]
    public Review Review { get; set; } = null!;

    [JsonPropertyName("reviews")]
    public Connection<Review> Reviews { get; set; } = null!;

    [JsonPropertyName("version")]
    public Versions.Version Version { get; set; } = null!;

    [JsonPropertyName("createReview")]
    public Review CreateReview { get; set; } = null!;

    [JsonPropertyName("deleteReview")]
    public bool? DeleteReview { get; set; }

    [JsonPropertyName("updateReview")]
    public bool? UpdateReview { get; set; }
}
