namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Reviews.Delete;

using System;
using System.Text.Json.Serialization;

public partial class DeleteReviewInputData
{
    [JsonPropertyName("query")]
    public string Query { get; set; } = null!;

    [JsonPropertyName("variables")]
    public Variables Variables { get; set; } = null!;
}

public partial class Variables
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}
