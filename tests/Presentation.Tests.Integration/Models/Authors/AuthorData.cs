namespace CleanGraphQL.Presentation.Tests.Integration.Models.Authors;

using System;
using System.Text.Json.Serialization;

public partial class Author
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("reviews")]
    public Review[] Reviews { get; set; }
}

public partial class Review
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("movie")]
    public Movie Movie { get; set; }
}

public partial class Movie
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}
