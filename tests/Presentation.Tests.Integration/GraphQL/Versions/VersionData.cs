namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Versions;

using System.Text.Json.Serialization;

public partial class Version
{
    [JsonPropertyName("fileVersion")]
    public string FileVersion { get; set; }

    [JsonPropertyName("informationalVersion")]
    public string InformationalVersion { get; set; }
}
