namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Authors;

using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Shouldly;
using Xunit;

public class AuthorTests
{
    private static readonly GraphQLApplication Application = new();

    [Fact]
    public async Task Authors_ShouldReturn_AuthorsList()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(take:50){items{id,firstName,lastName,reviews{id,stars,movie{id,title}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Authors.Items.ShouldNotBeEmpty();
        result.Data.Authors.Items.Length.ShouldBe(15);

        foreach (var author in result.Data.Authors.Items)
        {
            _ = author.ShouldNotBeNull();
            _ = author.Id.ShouldBeOfType<Guid>();
            _ = author.FirstName.ShouldBeOfType<string>();
            author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = author.LastName.ShouldBeOfType<string>();
            author.LastName.ShouldNotBeNullOrWhiteSpace();

            foreach (var review in author.Reviews.ShouldNotBeNull())
            {
                _ = review.Stars.ShouldBeOfType<int>();
                review.Stars.ShouldBeInRange(1, 5);

                _ = review.Movie.ShouldNotBeNull();
                _ = review.Movie.Id.ShouldBeOfType<Guid>();
                _ = review.Movie.Title.ShouldBeOfType<string>();
                review.Movie.Title.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }

    [Fact]
    public async Task Author_ShouldReturn_Author()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={authors(take:50){items{id}}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var authorId = setupResult.Data.Authors.Items[0].Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{author(id:\"{authorId}\"){{id,firstName,lastName,reviews{{id,stars,movie{{id,title}}}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.Author.ShouldNotBeNull();
        result.Data.Author.Id.ShouldBe(authorId);
        _ = result.Data.Author.FirstName.ShouldBeOfType<string>();
        result.Data.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.Data.Author.LastName.ShouldBeOfType<string>();
        result.Data.Author.LastName.ShouldNotBeNullOrWhiteSpace();
        result.Data.Author.Reviews.ShouldNotBeEmpty();

        foreach (var review in result.Data.Author.Reviews)
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.Movie.ShouldNotBeNull();
            _ = review.Movie.Id.ShouldBeOfType<Guid>();
            _ = review.Movie.Title.ShouldBeOfType<string>();
            review.Movie.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Author_ShouldReturn_ValidationError()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/graphql?query={{author(id:\"{Guid.Empty}\"){{id}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
        result.Errors[0].Extensions.Errors["Id"].ShouldBe(["An Author Id was not supplied."]);
    }

    [Fact]
    public async Task Author_ShouldReturn_NotFoundError()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync($"/graphql?query={{author(id:\"{Guid.NewGuid()}\"){{id}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("The Author with the supplied id was not found.");
        result.Errors[0].Extensions.Code.ShouldBe("NOT_FOUND");
    }

    [Fact]
    public async Task Authors_ShouldReturn_FirstPage()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(take:5){totalCount,pageInfo{hasNextPage,hasPreviousPage},items{id}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Errors.ShouldBeNull();

        var page = result.Data.Authors;
        _ = page.ShouldNotBeNull();
        page.TotalCount.ShouldBe(15);
        page.Items.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeTrue();
        page.PageInfo.HasPreviousPage.ShouldBeFalse();
        page.Items.Select(i => i.Id).ShouldBeUnique();
    }

    [Fact]
    public async Task Authors_ShouldReturn_NextPage()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var firstResponse = await client.GetAsync("/graphql?query={authors(take:5){items{id}}}");
        var firstPage = (await firstResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors;

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(skip:5,take:5){totalCount,pageInfo{hasNextPage,hasPreviousPage},items{id}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Errors.ShouldBeNull();

        var page = result.Data.Authors;
        page.TotalCount.ShouldBe(15);
        page.Items.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeTrue();
        page.PageInfo.HasPreviousPage.ShouldBeTrue();
        page.Items.Select(i => i.Id).ShouldNotContain(id => firstPage.Items.Select(i => i.Id).Contains(id));
    }

    [Fact]
    public async Task Authors_ShouldReturn_LastPage()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(skip:10,take:10){totalCount,pageInfo{hasNextPage,hasPreviousPage},items{id}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Authors;
        page.Items.Length.ShouldBe(5);
        page.PageInfo.HasNextPage.ShouldBeFalse();
        page.PageInfo.HasPreviousPage.ShouldBeTrue();
    }

    [Fact]
    public async Task Authors_ShouldFilter_ByQuery()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={authors(take:1){items{lastName}}}");
        var term = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors.Items[0].LastName.Split(' ')[0].ToUpperInvariant();

        // Act
        using var response = await client.GetAsync($"/graphql?query={{authors(query:\"{term}\",take:50){{totalCount,items{{lastName}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Authors;
        page.TotalCount.ShouldBeGreaterThan(0);
        page.Items.Length.ShouldBe(page.TotalCount);

        foreach (var item in page.Items)
        {
            item.LastName.ShouldContain(term, Case.Insensitive);
        }
    }

    [Fact]
    public async Task Authors_ShouldReturn_EmptyPage_WhenNothingMatches()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(query:\"zzzz-no-such-match\"){totalCount,pageInfo{hasNextPage,hasPreviousPage},items{id}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Authors;
        page.TotalCount.ShouldBe(0);
        page.Items.ShouldBeEmpty();
        page.PageInfo.HasNextPage.ShouldBeFalse();
        page.PageInfo.HasPreviousPage.ShouldBeFalse();
    }

    [Fact]
    public async Task Authors_ShouldReturn_ValidationError_ForNegativeSkip()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(skip:-1){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Authors.ShouldBeNull();

        result.Errors.ShouldNotBeNull().Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
        result.Errors[0].Extensions.Errors["Offset"].ShouldBe(["An offset can't be negative. Use 0 for the first page."]);
    }

    [Fact]
    public async Task Authors_ShouldReturn_Error_WhenPageIsTooLarge()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={authors(take:51){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Authors.ShouldBeNull();

        result.Errors.ShouldNotBeNull().Count.ShouldBe(1);
        result.Errors[0].Extensions.Code.ShouldBe("HC0051");
    }
}
