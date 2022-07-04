namespace CleanGraphQLApi.Presentation.Tests.Integration.GraphQL.Reviews;

using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Create;
using Delete;
using Shouldly;
using Update;
using Xunit;
using Review = Create.Review;
using Variables = Create.Variables;

public class ReviewTests
{
    private static readonly GraphQLApplication Application = new();

    #region Create

    [Fact]
    public async Task Create_ShouldCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/graphql?query={authors{id,firstName,lastName}}");
        var author = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors[0];
        using var movieResponse = await client.GetAsync("/graphql?query={movies{id,title}}");
        var movie = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies[0];
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,dateCreated,dateModified,author{id,firstName,lastName,dateCreated,dateModified,},movie{id,title,dateCreated,dateModified}}}",
            Variables = new Variables
            {
                Review = new Review
                {
                    AuthorId = author.Id,
                    MovieId = movie.Id,
                    Stars = 5
                }
            }
        };

        // Act
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
        result.Data.CreateReview.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.DateCreated.ShouldNotBe(default);
        result.Data.CreateReview.DateModified.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.DateModified.ShouldNotBe(default);

        _ = result.Data.CreateReview.Author.ShouldNotBeNull();
        result.Data.CreateReview.Author.Id.ShouldBe(author.Id);
        result.Data.CreateReview.Author.FirstName.ShouldBe(author.FirstName);
        result.Data.CreateReview.Author.LastName.ShouldBe(author.LastName);
        result.Data.CreateReview.Author.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.Author.DateCreated.ShouldNotBe(default);
        result.Data.CreateReview.Author.DateModified.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.Author.DateModified.ShouldNotBe(default);

        _ = result.Data.CreateReview.Movie.ShouldNotBeNull();
        result.Data.CreateReview.Movie.Id.ShouldBe(movie.Id);
        result.Data.CreateReview.Movie.Title.ShouldBe(movie.Title);
        result.Data.CreateReview.Movie.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.Movie.DateCreated.ShouldNotBe(default);
        result.Data.CreateReview.Movie.DateModified.ShouldBeOfType<DateTime>();
        result.Data.CreateReview.Movie.DateModified.ShouldNotBe(default);
    }

    [Fact]
    public async Task Create_ShouldNotCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,dateCreated,dateModified,author{id,firstName,lastName,dateCreated,dateModified,},movie{id,title,dateCreated,dateModified}}}",
            Variables = new Variables
            {
                Review = new Review
                {
                    AuthorId = Guid.Empty,
                    MovieId = Guid.Empty,
                    Stars = 5
                }
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.Data.ShouldNotBeNull();
        result.Data.CreateReview.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("A an error occured while attempting to create the review.");
    }

    [Fact]
    public async Task CreateNull_ShouldNotCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,dateCreated,dateModified,author{id,firstName,lastName,dateCreated,dateModified,},movie{id,title,dateCreated,dateModified}}}",
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Data.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("GraphQL.Validation.InvalidVariableError");
        result.Errors[0].Message.ShouldContain("No value provided for a non-null variable.");
    }

    #endregion Create

    #region Delete

    [Fact]
    public async Task Delete_ShouldDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/graphql?query={reviews{id}}");
        var reviewId = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews[0].Id;
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:ID!){deleteReview(id:$id)}",
            Variables = new Delete.Variables
            {
                Id = reviewId
            }
        };

        // Act
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
    public async Task Delete_ShouldNotDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:ID!){deleteReview(id:$id)}",
            Variables = new Delete.Variables
            {
                Id = Guid.Empty
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var resultS = await response.Content.ReadAsStringAsync();
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.Data.ShouldNotBeNull();
        result.Data.DeleteReview.ShouldBeFalse();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("A an error occured while attempting to delete the review.");
    }

    [Fact]
    public async Task DeleteNull_ShouldNotDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:ID!){deleteReview(id:$id)}"
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Data.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("GraphQL.Validation.InvalidVariableError");
        result.Errors[0].Message.ShouldContain("No value provided for a non-null variable.");
    }

    #endregion Delete

    [Fact]
    public async Task Reviews_ShouldReturn_ReviewsList()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={reviews{id,stars,dateCreated,dateModified,author{id,firstName,lastName,dateCreated,dateModified},movie{id,title,dateCreated,dateModified}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Reviews.ShouldNotBeEmpty();
        result.Data.Reviews.Length.ShouldBeGreaterThan(145);

        foreach (var review in result.Data.Reviews)
        {
            _ = review.ShouldNotBeNull();
            _ = review.Id.ShouldBeOfType<Guid>();
            _ = review.Stars.ShouldBeOfType<int>();
            review.Stars.ShouldBeInRange(1, 5);
            review.Author.DateCreated.ShouldBeOfType<DateTime>();
            review.Author.DateCreated.ShouldNotBe(default);
            review.Author.DateModified.ShouldBeOfType<DateTime>();
            review.Author.DateModified.ShouldNotBe(default);

            _ = review.Author.ShouldNotBeNull();
            _ = review.Author.Id.ShouldBeOfType<Guid>();
            _ = review.Author.FirstName.ShouldBeOfType<string>();
            review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
            _ = review.Author.LastName.ShouldBeOfType<string>();
            review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
            review.Author.DateCreated.ShouldBeOfType<DateTime>();
            review.Author.DateCreated.ShouldNotBe(default);
            review.Author.DateModified.ShouldBeOfType<DateTime>();
            review.Author.DateModified.ShouldNotBe(default);

            _ = review.Movie.ShouldNotBeNull();
            _ = review.Movie.Id.ShouldBeOfType<Guid>();
            _ = review.Movie.Title.ShouldBeOfType<string>();
            review.Movie.Title.ShouldNotBeNullOrWhiteSpace();
            review.Movie.DateCreated.ShouldBeOfType<DateTime>();
            review.Movie.DateCreated.ShouldNotBe(default);
            review.Movie.DateModified.ShouldBeOfType<DateTime>();
            review.Movie.DateModified.ShouldNotBe(default);
        }
    }

    [Fact]
    public async Task Review_ShouldReturn_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={reviews{id}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var reviewId = setupResult.Data.Reviews[0].Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{review(id:\"{reviewId}\"){{id,stars,dateCreated,dateModified,author{{id,firstName,lastName,dateCreated,dateModified}},movie{{id,title,dateCreated,dateModified}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        _ = result.Data.Review.ShouldNotBeNull();
        _ = result.Data.Review.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Stars.ShouldBeOfType<int>();
        result.Data.Review.Stars.ShouldBeInRange(1, 5);
        result.Data.Review.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.Review.DateCreated.ShouldNotBe(default);
        result.Data.Review.DateModified.ShouldBeOfType<DateTime>();
        result.Data.Review.DateModified.ShouldNotBe(default);

        _ = result.Data.Review.Author.ShouldNotBeNull();
        _ = result.Data.Review.Author.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Author.FirstName.ShouldBeOfType<string>();
        result.Data.Review.Author.FirstName.ShouldNotBeNullOrWhiteSpace();
        _ = result.Data.Review.Author.LastName.ShouldBeOfType<string>();
        result.Data.Review.Author.LastName.ShouldNotBeNullOrWhiteSpace();
        result.Data.Review.Author.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.Review.Author.DateCreated.ShouldNotBe(default);
        result.Data.Review.Author.DateModified.ShouldBeOfType<DateTime>();
        result.Data.Review.Author.DateModified.ShouldNotBe(default);

        _ = result.Data.Review.Movie.ShouldNotBeNull();
        _ = result.Data.Review.Movie.Id.ShouldBeOfType<Guid>();
        _ = result.Data.Review.Movie.Title.ShouldBeOfType<string>();
        result.Data.Review.Movie.DateCreated.ShouldBeOfType<DateTime>();
        result.Data.Review.Movie.DateCreated.ShouldNotBe(default);
        result.Data.Review.Movie.DateModified.ShouldBeOfType<DateTime>();
        result.Data.Review.Movie.DateModified.ShouldNotBe(default);
    }

    #region Update

    [Fact]
    public async Task Update_ShouldUpdate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var authorResponse = await client.GetAsync("/graphql?query={authors{id}}");
        var author = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors[0];
        using var movieResponse = await client.GetAsync("/graphql?query={movies{id}}");
        var movie = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies[0];
        using var reviewResponse = await client.GetAsync("/graphql?query={reviews{id}}");
        var review = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews[0];
        var content = new UpdateReviewInputData()
        {
            Query = "mutation($review:UpdateReviewInput!){updateReview(input:$review)}",
            Variables = new Update.Variables
            {
                Review = new Update.Review
                {
                    Id = review.Id,
                    AuthorId = author.Id,
                    MovieId = movie.Id,
                    Stars = 5
                }
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.UpdateReview.ShouldBeTrue();
    }

    [Fact]
    public async Task Update_ShouldNotUpdate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new UpdateReviewInputData()
        {
            Query = "mutation($review:UpdateReviewInput!){updateReview(input:$review)}",
            Variables = new Update.Variables
            {
                Review = new Update.Review
                {
                    Id = Guid.Empty,
                    AuthorId = Guid.Empty,
                    MovieId = Guid.Empty,
                    Stars = 5
                }
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.Data.ShouldNotBeNull();
        result.Data.UpdateReview.ShouldBeFalse();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("A an error occured while attempting to update the review.");
    }

    [Fact]
    public async Task UpdateNull_ShouldNotDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new UpdateReviewInputData()
        {
            Query = "mutation($review:UpdateReviewInput!){updateReview(input:$review)}"
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        _ = result.ShouldNotBeNull();

        result.Data.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldContain("GraphQL.Validation.InvalidVariableError");
        result.Errors[0].Message.ShouldContain("No value provided for a non-null variable.");
    }

    #endregion Update
}
