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
        using var response = await client.GetAsync("/graphql?query={movies{id,title,dateCreated,dateModified,reviews{id,stars,dateCreated,dateModified,author{id,firstName,lastName,dateCreated,dateModified}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Movies.ShouldNotBeEmpty();
        result.Data.Movies.Length.ShouldBe(50);

        foreach (var movie in result.Data.Movies)
        {
            _ = movie.ShouldNotBeNull();
            _ = movie.Id.ShouldBeOfType<Guid>();
            _ = movie.Title.ShouldBeOfType<string>();
            movie.Title.ShouldNotBeNullOrWhiteSpace();
            _ = movie.DateCreated.ShouldBeOfType<DateTime>();
            movie.DateCreated.ShouldNotBe(default);
            _ = movie.DateModified.ShouldBeOfType<DateTime>();
            movie.DateModified.ShouldNotBe(default);

            foreach (var review in movie.Reviews)
            {
                _ = review.Stars.ShouldBeOfType<int>();
                review.Stars.ShouldBeInRange(1, 5);
                _ = review.DateCreated.ShouldBeOfType<DateTime>();
                review.DateCreated.ShouldNotBe(default);
                _ = review.DateModified.ShouldBeOfType<DateTime>();
                review.DateModified.ShouldNotBe(default);

                _ = review.Author.ShouldNotBeNull();
                _ = review.Author.Id.ShouldBeOfType<Guid>();
                _ = review.Author.FirstName.ShouldBeOfType<string>();
                review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
                _ = review.Author.LastName.ShouldBeOfType<string>();
                review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
                _ = review.Author.DateCreated.ShouldBeOfType<DateTime>();
                review.Author.DateCreated.ShouldNotBe(default);
                _ = review.Author.DateModified.ShouldBeOfType<DateTime>();
                review.Author.DateModified.ShouldNotBe(default);
            }
        }
    }

    [Fact]
    public async Task Movies_ShouldReturn_Movie()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={movies{id}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var movieId = setupResult.Data.Movies[0].Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{movie(id:\"{movieId}\"){{id,title,dateCreated,dateModified,reviews{{id,stars,dateCreated,dateModified,author{{id,firstName,lastName,dateCreated,dateModified}}}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.Movie.ShouldNotBeNull();
        result.Data.Movie.Id.ShouldBe(movieId);
        _ = result.Data.Movie.Title.ShouldBeOfType<string>();
        result.Data.Movie.Title.ShouldNotBeNullOrWhiteSpace();
        _ = result.Data.Movie.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.Movie.DateCreated.ShouldNotBe(default);
        _ = result.Data.Movie.DateModified.ShouldBeOfType<DateTime>();
        result.Data.Movie.DateModified.ShouldNotBe(default);
        result.Data.Movie.Reviews.ShouldNotBeEmpty();

        foreach (var review in result.Data.Movie.Reviews)
        {
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);
            _ = review.DateCreated.ShouldBeOfType<DateTime>();
            review.DateCreated.ShouldNotBe(default);
            _ = review.DateModified.ShouldBeOfType<DateTime>();
            review.DateModified.ShouldNotBe(default);

            _ = review.Author.ShouldNotBeNull();
            _ = review.Author.Id.ShouldBeOfType<Guid>();
            _ = review.Author.FirstName.ShouldBeOfType<string>();
            review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.Author.LastName.ShouldBeOfType<string>();
            review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
            _ = review.Author.DateCreated.ShouldBeOfType<DateTime>();
            review.Author.DateCreated.ShouldNotBe(default);
            _ = review.Author.DateModified.ShouldBeOfType<DateTime>();
            review.Author.DateModified.ShouldNotBe(default);
        }
    }
}
