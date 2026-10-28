namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class ErrorMessage
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("extensions")]
    public ErrorExtensions Extensions { get; set; } = null!;
}

public partial class ErrorExtensions
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; set; } = null!;
}
