namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Common;

using System.Text.Json.Serialization;

public partial class ErrorMessage
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
