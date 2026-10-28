namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Movies;

using System.Net;
using System.Threading.Tasks;
using Common;
using Shouldly;
using Xunit;

public class MovieTests
{
    private static readonly GraphQLApplication Application = new();

    [Fact]
    public async Task Movies_ShouldReturn_MoviesList()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={movies(first:50){nodes{id,title,reviews{id,stars,author{id,firstName,lastName}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Movies.Nodes.ShouldNotBeEmpty();
        result.Data.Movies.Nodes.Length.ShouldBe(50);

        foreach (var movie in result.Data.Movies.Nodes)
        {
            _ = movie.ShouldNotBeNull();
            _ = movie.Id.ShouldBeOfType<Guid>();
            _ = movie.Title.ShouldBeOfType<string>();
            movie.Title.ShouldNotBeNullOrWhiteSpace();

            foreach (var review in movie.Reviews)
            {
                _ = review.Stars.ShouldBeOfType<int>();
                review.Stars.ShouldBeInRange(1, 5);

                _ = review.Author.ShouldNotBeNull();
                _ = review.Author.Id.ShouldBeOfType<Guid>();
                _ = review.Author.FirstName.ShouldBeOfType<string>();
                review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
                _ = review.Author.LastName.ShouldBeOfType<string>();
                review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }

    [Fact]
    public async Task Movies_ShouldReturn_Movie()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={movies(first:50){nodes{id,reviews{id}}}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var movieId = setupResult.Data.Movies.Nodes.First(m => m.Reviews.Length > 0).Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movie(id:\"{movieId}\"){{id,title,reviews{{id,stars,author{{id,firstName,lastName}}}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.Movie.ShouldNotBeNull();
        result.Data.Movie.Id.ShouldBe(movieId);
        _ = result.Data.Movie.Title.ShouldBeOfType<string>();
        result.Data.Movie.Title.ShouldNotBeNullOrWhiteSpace();
        result.Data.Movie.Reviews.ShouldNotBeEmpty();

        foreach (var review in result.Data.Movie.Reviews)
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.Author.ShouldNotBeNull();
            _ = review.Author.Id.ShouldBeOfType<Guid>();
            _ = review.Author.FirstName.ShouldBeOfType<string>();
            review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.Author.LastName.ShouldBeOfType<string>();
            review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Movie_ShouldReturn_NotFoundError()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movie(id:\"{Guid.NewGuid()}\"){{id}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();
        result.Data.Movie.ShouldBeNull();

        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("The Movie with the supplied id was not found.");
        result.Errors[0].Extensions.Code.ShouldBe("NOT_FOUND");
    }

    [Fact]
    public async Task Movies_ShouldReturn_FirstPage()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={movies(first:5){totalCount,pageInfo{hasNextPage,hasPreviousPage,startCursor,endCursor},edges{cursor,node{id,title}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Errors.ShouldBeNull();

        var page = result.Data.Movies;
        _ = page.ShouldNotBeNull();
        page.TotalCount.ShouldBe(50);
        page.Edges.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeTrue();
        page.PageInfo.HasPreviousPage.ShouldBeFalse();
        page.PageInfo.StartCursor.ShouldBe(page.Edges[0].Cursor);
        page.PageInfo.EndCursor.ShouldBe(page.Edges[^1].Cursor);
        page.Edges.Select(e => e.Cursor).ShouldBeUnique();

        foreach (var edge in page.Edges)
        {
            edge.Node.Id.ShouldNotBe(Guid.Empty);
            edge.Node.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Movies_ShouldReturn_NextPage()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var firstResponse = await client.GetAsync("/graphql?query={movies(first:5){pageInfo{endCursor},nodes{id}}}");
        var firstPage = (await firstResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movies(first:5,after:\"{firstPage.PageInfo.EndCursor}\"){{totalCount,pageInfo{{hasNextPage,hasPreviousPage}},nodes{{id}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Errors.ShouldBeNull();

        var page = result.Data.Movies;
        page.TotalCount.ShouldBe(50);
        page.Nodes.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeTrue();
        page.PageInfo.HasPreviousPage.ShouldBeTrue();
        page.Nodes.Select(m => m.Id).ShouldNotContain(id => firstPage.Nodes.Select(m => m.Id).Contains(id));
    }

    [Fact]
    public async Task Movies_ShouldReturn_LastPage()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var firstResponse = await client.GetAsync("/graphql?query={movies(first:45){pageInfo{endCursor}}}");
        var firstPage = (await firstResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movies(first:10,after:\"{firstPage.PageInfo.EndCursor}\"){{totalCount,pageInfo{{hasNextPage}},nodes{{id}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Movies;
        page.Nodes.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeFalse();
    }

    [Fact]
    public async Task Movies_ShouldFilter_ByQuery()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={movies(first:50){nodes{title}}}");
        var term = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies.Nodes[0].Title.Split(' ')[0].ToUpperInvariant();

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movies(query:\"{term}\",first:50){{totalCount,nodes{{title}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Movies;
        page.TotalCount.ShouldBeGreaterThan(0);
        page.Nodes.Length.ShouldBe(page.TotalCount);

        foreach (var movie in page.Nodes)
        {
            movie.Title.ShouldContain(term, Case.Insensitive);
        }
    }

    [Fact]
    public async Task Movies_ShouldReturn_ValidationError_ForInvalidCursor()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={movies(after:\"not-a-cursor\"){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Movies.ShouldBeNull();

        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
        result.Errors[0].Extensions.Errors["After"].ShouldBe(["The cursor is not valid. Use the endCursor from a previous page, or leave it empty for the first page."]);
    }

    [Fact]
    public async Task Movies_ShouldReturn_Error_WhenPageIsTooLarge()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={movies(first:51){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Movies.ShouldBeNull();

        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Extensions.Code.ShouldBe("HC0051");
    }

    [Fact]
    public async Task Movies_ShouldReturn_EmptyPage_WhenNothingMatches()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={movies(query:\"zzzz-no-such-movie\"){totalCount,pageInfo{hasNextPage,hasPreviousPage,startCursor,endCursor},nodes{id}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Movies;
        page.TotalCount.ShouldBe(0);
        page.Nodes.ShouldBeEmpty();
        page.PageInfo.HasNextPage.ShouldBeFalse();
        page.PageInfo.HasPreviousPage.ShouldBeFalse();
        page.PageInfo.StartCursor.ShouldBeNull();
        page.PageInfo.EndCursor.ShouldBeNull();
    }
}
