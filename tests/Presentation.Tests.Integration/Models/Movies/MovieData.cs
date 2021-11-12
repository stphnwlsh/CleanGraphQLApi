namespace CleanGraphQL.Presentation.Tests.Integration.Models.Movies;

using System;
using System.Text.Json.Serialization;

public partial class Movie
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("reviews")]
    public Review[] Reviews { get; set; }
}

public partial class Review
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }
}

public partial class Author
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
}
