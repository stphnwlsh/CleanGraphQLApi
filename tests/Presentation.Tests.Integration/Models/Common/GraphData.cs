namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Common;

using System.Text.Json.Serialization;

public partial class GraphData
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }

    [JsonPropertyName("extensions")]
    public Extensions Extensions { get; set; }
}
