namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class Execution
{
    [JsonPropertyName("resolvers")]
    public object[] Resolvers { get; set; }
}
