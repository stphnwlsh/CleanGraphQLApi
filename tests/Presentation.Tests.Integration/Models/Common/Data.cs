namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Common;

using System.Text.Json.Serialization;
using CleanGraphQLApi.Presentation.Tests.Integration.Models.Reviews.Query;

public partial class Data
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
}
