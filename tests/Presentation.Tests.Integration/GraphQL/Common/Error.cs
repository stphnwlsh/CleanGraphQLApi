namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class ErrorMessage
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("extensions")]
    public ErrorExtensions Extensions { get; set; }
}

public partial class ErrorExtensions
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; set; }
}
