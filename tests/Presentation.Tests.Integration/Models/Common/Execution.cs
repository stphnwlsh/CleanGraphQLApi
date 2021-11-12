namespace CleanGraphQL.Presentation.Tests.Integration.Models.Common;

using System.Text.Json.Serialization;

public partial class Execution
{
    [JsonPropertyName("resolvers")]
    public object[] Resolvers { get; set; }
}
