namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Reviews.Update;

using System;
using System.Text.Json.Serialization;

public partial class UpdateReviewInputData
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
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("movieId")]
    public Guid MovieId { get; set; }

    [JsonPropertyName("authorId")]
    public Guid AuthorId { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }
}
