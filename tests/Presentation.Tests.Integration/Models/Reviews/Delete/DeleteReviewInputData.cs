namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Reviews.Delete;

using System;
using System.Text.Json.Serialization;

public partial class DeleteReviewInputData
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("variables")]
    public Variables Variables { get; set; }
}

public partial class Variables
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}
