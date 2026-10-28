namespace CleanGraphQLApi.Presentation.GraphQL.Extensions;

using System.Globalization;
using System.Text;
using Application.Common.Paging;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate.Types.Pagination;

/// <summary>
/// Turns offset pages from the Application layer into Relay connections. Each cursor holds the offset of the result after it.
/// </summary>
public static class PageExtensions
{
    private const string Prefix = "offset:";

    /// <summary>
    /// Reads the offset from the cursor sent as the after argument. An empty cursor is the first page.
    /// </summary>
    /// <param name="after">The endCursor from a previous page, or null for the first page.</param>
    /// <exception cref="ValidationException">The cursor did not come from a previous page.</exception>
    public static int ToOffset(this string? after)
    {
        if (string.IsNullOrEmpty(after))
        {
            return 0;
        }

        var bytes = new byte[after.Length];

        if (Convert.TryFromBase64String(after, bytes, out var length)
            && Encoding.UTF8.GetString(bytes, 0, length) is var value
            && value.StartsWith(Prefix, StringComparison.Ordinal)
            && int.TryParse(value.AsSpan(Prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out var offset))
        {
            return offset;
        }

        throw new ValidationException(
        [
            new ValidationFailure("After", "The cursor is not valid. Use the endCursor from a previous page, or leave it empty for the first page.")
        ]);
    }

    /// <summary>
    /// Converts a page from the Application layer to a GraphQL connection.
    /// </summary>
    /// <param name="page">The page of results.</param>
    public static Connection<T> ToConnection<T>(this Page<T> page)
    {
        var edges = page.Items
            .Select((item, index) => new Edge<T>(item, ToCursor(page.Offset + index + 1)))
            .ToList();

        var pageInfo = new ConnectionPageInfo(
            hasNextPage: page.Offset + page.Items.Count < page.TotalCount,
            hasPreviousPage: page.Offset > 0,
            startCursor: edges.FirstOrDefault()?.Cursor,
            endCursor: edges.LastOrDefault()?.Cursor);

        return new Connection<T>(edges, pageInfo, page.TotalCount);
    }

    private static string ToCursor(int offset)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(Prefix + offset.ToString(CultureInfo.InvariantCulture)));
    }
}
