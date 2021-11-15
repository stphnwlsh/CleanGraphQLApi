namespace CleanGraphQLApi.Presentation.Tests.Integration.Models.Common;

using System.Text.Json.Serialization;

public partial class Tracing
{
    [JsonPropertyName("version")]
    public long Version { get; set; }

    [JsonPropertyName("startTime")]
    public DateTimeOffset StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("parsing")]
    public Parsing Parsing { get; set; }

    [JsonPropertyName("validation")]
    public Parsing Validation { get; set; }

    [JsonPropertyName("execution")]
    public Execution Execution { get; set; }
}
