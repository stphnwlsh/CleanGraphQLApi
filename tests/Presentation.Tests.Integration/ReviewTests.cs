namespace CleanGraphQL.Presentation.Tests.Integration;

using System.Net;
using System.Threading.Tasks;
using CleanGraphQL.Presentation.Tests.Integration.Models.Common;
using Shouldly;
using Xunit;

public class ReviewTests
{
    private static readonly GraphQLApplication Application = new();

    [Fact]
    public async Task Reviews_ShouldCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/graphql?query={authors{id,firstName,lastName}}");
        var author = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors[0];
        using var movieResponse = await client.GetAsync("/graphql?query={movies{id,title}}");
        var movie = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies[0];

        // Act
        var content = new Models.Reviews.Create.ReviewInputData
        {
            Query = "mutation($review:ReviewInput!){createReview(review:$review){id,stars,author{id,firstName,lastName},movie{id,title}}}",
            Variables = new Models.Reviews.Create.Variables
            {
                Review = new Models.Reviews.Create.Review
                {
                    AuthorId = author.Id,
                    MovieId = movie.Id,
                    Stars = 5
                }
            }
        };
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.CreateReview.ShouldNotBeNull();
        _ = result.Data.CreateReview.Id.ShouldBeOfType<Guid>();
        _ = result.Data.CreateReview.Stars.ShouldBeOfType<int>();
        result.Data.CreateReview.Stars.ShouldBe(5);

        _ = result.Data.CreateReview.Author.ShouldNotBeNull();
        result.Data.CreateReview.Author.Id.ShouldBe(author.Id);
        result.Data.CreateReview.Author.FirstName.ShouldBe(author.FirstName);
        result.Data.CreateReview.Author.LastName.ShouldBe(author.LastName);

        _ = result.Data.CreateReview.Movie.ShouldNotBeNull();
        result.Data.CreateReview.Movie.Id.ShouldBe(movie.Id);
        result.Data.CreateReview.Movie.Title.ShouldBe(movie.Title);
    }

    [Fact]
    public async Task Reviews_ShouldDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/graphql?query={reviews{id}}");
        var reviewId = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews[0].Id;

        // Act
        var content = new Models.Reviews.Delete.ReviewDeleteData
        {
            Query = "mutation($id:ID!){deleteReview(id:$id)}",
            Variables = new Models.Reviews.Delete.Variables
            {
                Id = reviewId
            }
        };
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var resultS = await response.Content.ReadAsStringAsync();
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.DeleteReview.ShouldBeTrue();
    }

    [Fact]
    public async Task Reviews_ShouldReturn_ReviewsList()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={reviews{id,stars,author{id,firstName,lastName},movie{id,title}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Reviews.ShouldNotBeEmpty();
        result.Data.Reviews.Length.ShouldBe(150);

        foreach (var review in result.Data.Reviews)
        {
            _ = review.ShouldNotBeNull();
            _ = review.Id.ShouldBeOfType<Guid>();
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);

            _ = review.Author.ShouldNotBeNull();
            _ = review.Author.Id.ShouldBeOfType<Guid>();
            _ = review.Author.FirstName.ShouldBeOfType<string>();
            review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.Author.LastName.ShouldBeOfType<string>();
            review.Author.LastName.ShouldNotBeNullOrWhiteSpace();

            _ = review.Movie.ShouldNotBeNull();
            _ = review.Movie.Id.ShouldBeOfType<Guid>();
            _ = review.Movie.Title.ShouldBeOfType<string>();
            review.Movie.Title.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Movies_ShouldReturn_Movie()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={reviews{id}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var reviewId = setupResult.Data.Reviews[0].Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{review(id:\"{reviewId}\"){{id,stars,author{{id,firstName,lastName}},movie{{id,title}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.Review.ShouldNotBeNull();
        _ = result.Data.Review.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Stars.ShouldBeOfType<int>();
        result.Data.Review.Stars.ShouldBeInRange(1, 5);

        _ = result.Data.Review.Author.ShouldNotBeNull();
        _ = result.Data.Review.Author.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Author.FirstName.ShouldBeOfType<string>();
        result.Data.Review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.Data.Review.Author.LastName.ShouldBeOfType<string>();
        result.Data.Review.Author.LastName.ShouldNotBeNullOrWhiteSpace();

        _ = result.Data.Review.Movie.ShouldNotBeNull();
        _ = result.Data.Review.Movie.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Movie.Title.ShouldBeOfType<string>();
    }
}
