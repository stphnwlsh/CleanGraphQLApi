namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public abstract partial class ErrorMessage
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
