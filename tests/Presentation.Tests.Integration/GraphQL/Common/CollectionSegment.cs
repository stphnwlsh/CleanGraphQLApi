namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class CollectionSegment<T>
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageInfo")]
    public CollectionSegmentInfo PageInfo { get; set; } = null!;

    [JsonPropertyName("items")]
    public T[] Items { get; set; } = [];
}

public partial class CollectionSegmentInfo
{
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }
}
