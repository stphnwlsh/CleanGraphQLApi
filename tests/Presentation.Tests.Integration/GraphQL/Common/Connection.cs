namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Common;

using System.Text.Json.Serialization;

public partial class Connection<T>
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageInfo")]
    public PageInfo PageInfo { get; set; } = null!;

    [JsonPropertyName("edges")]
    public Edge<T>[] Edges { get; set; } = [];

    [JsonPropertyName("nodes")]
    public T[] Nodes { get; set; } = [];
}

public partial class Edge<T>
{
    [JsonPropertyName("cursor")]
    public string Cursor { get; set; } = null!;

    [JsonPropertyName("node")]
    public T Node { get; set; } = default!;
}

public partial class PageInfo
{
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }

    [JsonPropertyName("startCursor")]
    public string? StartCursor { get; set; }

    [JsonPropertyName("endCursor")]
    public string? EndCursor { get; set; }
}
