namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Reviews.Query;

using System;
using System.Text.Json.Serialization;

public partial class Review
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; } = null!;

    [JsonPropertyName("movie")]
    public Movie Movie { get; set; } = null!;
}

public partial class Author
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = null!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = null!;
}

public partial class Movie
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
}
