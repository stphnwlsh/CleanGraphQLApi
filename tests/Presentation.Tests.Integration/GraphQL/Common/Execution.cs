namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public abstract partial class Execution
{
    [JsonPropertyName("resolvers")]
    public object[] Resolvers { get; set; }
}
