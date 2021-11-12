namespace CleanGraphQL.Presentation.Tests.Integration;

using System.Net;
using System.Threading.Tasks;
using CleanGraphQL.Presentation.Tests.Integration.Models.Common;
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
        using var response = await client.GetAsync("/graphql?query={authors{id,firstName,lastName,reviews{id,stars,movie{id,title}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Authors.ShouldNotBeEmpty();
        result.Data.Authors.Length.ShouldBe(15);

        foreach (var author in result.Data.Authors)
        {
            _ = author.ShouldNotBeNull();
            _ = author.Id.ShouldBeOfType<Guid>();
            _ = author.FirstName.ShouldBeOfType<string>();
            author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = author.LastName.ShouldBeOfType<string>();
            author.LastName.ShouldNotBeNullOrWhiteSpace();

            foreach (var review in author.Reviews)
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
        using var setupResponse = await client.GetAsync("/graphql?query={authors{id}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var authorId = setupResult.Data.Authors[0].Id;

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
        result.Data.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
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
}
