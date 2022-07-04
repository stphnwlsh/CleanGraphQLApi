namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Reviews.Query;

using System;
using System.Text.Json.Serialization;

public partial class Review
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTime DateModified { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }

    [JsonPropertyName("movie")]
    public Movie Movie { get; set; }
}

public partial class Author
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTime DateModified { get; set; }
}

public partial class Movie
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTime DateModified { get; set; }
}
