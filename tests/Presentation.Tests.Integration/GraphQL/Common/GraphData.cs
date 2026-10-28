namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public partial class GraphData
{
    [JsonPropertyName("data")]
    public Data Data { get; set; } = null!;

    [JsonPropertyName("errors")]
    public List<ErrorMessage>? Errors { get; set; }
}
