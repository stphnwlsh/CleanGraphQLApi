namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class Extensions
{
    [JsonPropertyName("tracing")]
    public Tracing Tracing { get; set; }
}
