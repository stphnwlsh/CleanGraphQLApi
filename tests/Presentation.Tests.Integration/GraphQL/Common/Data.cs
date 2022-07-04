namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;
using Reviews.Query;

public abstract partial class Data
{
    [JsonPropertyName("author")]
    public Authors.Author Author { get; set; }

    [JsonPropertyName("authors")]
    public Authors.Author[] Authors { get; set; }

    [JsonPropertyName("movie")]
    public Movies.Movie Movie { get; set; }

    [JsonPropertyName("movies")]
    public Movies.Movie[] Movies { get; set; }

    [JsonPropertyName("review")]
    public Review Review { get; set; }

    [JsonPropertyName("reviews")]
    public Review[] Reviews { get; set; }

    [JsonPropertyName("createReview")]
    public Review CreateReview { get; set; }

    [JsonPropertyName("deleteReview")]
    public bool DeleteReview { get; set; }

    [JsonPropertyName("updateReview")]
    public bool UpdateReview { get; set; }
}
