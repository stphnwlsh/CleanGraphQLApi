namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public abstract partial class Parsing
{
    [JsonPropertyName("startOffset")]
    public long StartOffset { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }
}
