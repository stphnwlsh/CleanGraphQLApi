namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Reviews.Create;

using System;
using System.Text.Json.Serialization;

public partial class ReviewInputData
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("variables")]
    public Variables Variables { get; set; }
}

public partial class Variables
{
    [JsonPropertyName("review")]
    public Review Review { get; set; }
}

public partial class Review
{
    [JsonPropertyName("movieId")]
    public Guid MovieId { get; set; }

    [JsonPropertyName("authorId")]
    public Guid AuthorId { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }
}
