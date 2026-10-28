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
        using var authorResponse = await client.GetAsync("/graphql?query={authors(first:50){nodes{id,firstName,lastName}}}");
        var author = (await authorResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Authors.Nodes[0];
        using var movieResponse = await client.GetAsync("/graphql?query={movies(first:50){nodes{id,title}}}");
        var movie = (await movieResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Movies.Nodes[0];
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,author{id,firstName,lastName,},movie{id,title}}}",
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

        _ = result.Data.CreateReview.Author.ShouldNotBeNull();
        result.Data.CreateReview.Author.Id.ShouldBe(author.Id);
        result.Data.CreateReview.Author.FirstName.ShouldBe(author.FirstName);
        result.Data.CreateReview.Author.LastName.ShouldBe(author.LastName);

        _ = result.Data.CreateReview.Movie.ShouldNotBeNull();
        result.Data.CreateReview.Movie.Id.ShouldBe(movie.Id);
        result.Data.CreateReview.Movie.Title.ShouldBe(movie.Title);
    }

    [Fact]
    public async Task Create_ShouldNotCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,author{id,firstName,lastName,},movie{id,title}}}",
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
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
    }

    [Fact]
    public async Task CreateNull_ShouldNotCreate_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new CreateReviewInputData
        {
            Query = "mutation($review:CreateReviewInput!){createReview(input:$review){id,stars,author{id,firstName,lastName,},movie{id,title}}}",
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
        result.Errors[0].Message.ShouldBe("Variable `review` is required.");
    }

    #endregion Create

    #region Delete

    [Fact]
    public async Task Delete_ShouldDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/graphql?query={reviews(first:50){nodes{id}}}");
        var reviewId = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews.Nodes[0].Id;
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:UUID!){deleteReview(id:$id)}",
            Variables = new Delete.Variables
            {
                Id = reviewId
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.DeleteReview.ShouldBe(true);
    }

    [Fact]
    public async Task Delete_ShouldNotDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:UUID!){deleteReview(id:$id)}",
            Variables = new Delete.Variables
            {
                Id = Guid.Empty
            }
        };

        // Act
        using var response = await client.PostAsync($"/graphql", content.ToStringContent());
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();

        _ = result.Data.ShouldNotBeNull();
        result.Data.DeleteReview.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
    }

    [Fact]
    public async Task DeleteNull_ShouldNotDelete_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        var content = new DeleteReviewInputData
        {
            Query = "mutation($id:UUID!){deleteReview(id:$id)}"
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
        result.Errors[0].Message.ShouldBe("Variable `id` is required.");
    }

    #endregion Delete

    [Fact]
    public async Task Reviews_ShouldReturn_ReviewsList()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={reviews(first:50){nodes{id,stars,author{id,firstName,lastName},movie{id,title}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        _ = result.Data.ShouldNotBeNull();

        result.Data.Reviews.Nodes.ShouldNotBeEmpty();
        result.Data.Reviews.Nodes.Length.ShouldBe(50);

        foreach (var review in result.Data.Reviews.Nodes)
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
    public async Task Review_ShouldReturn_Review()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={reviews(first:50){nodes{id}}}");
        var setupResult = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>();
        var reviewId = setupResult.Data.Reviews.Nodes[0].Id;

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

    #region Update

    [Fact]
    public async Task Update_ShouldUpdate_OnlyTheStars()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var reviewResponse = await client.GetAsync("/graphql?query={reviews(first:1){nodes{id,stars,author{id},movie{id}}}}");
        var review = (await reviewResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews.Nodes[0];
        var stars = review.Stars == 5 ? 1 : review.Stars + 1;
        var content = new UpdateReviewInputData()
        {
            Query = "mutation($review:UpdateReviewInput!){updateReview(input:$review)}",
            Variables = new Update.Variables
            {
                Review = new Update.Review
                {
                    Id = review.Id,
                    Stars = stars
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

        result.Data.UpdateReview.ShouldBe(true);

        using var updatedResponse = await client.GetAsync($"/graphql?query={{review(id:\"{review.Id}\"){{id,stars,author{{id}},movie{{id}}}}}}");
        var updated = (await updatedResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Review;

        updated.Stars.ShouldBe(stars);
        updated.Author.Id.ShouldBe(review.Author.Id);
        updated.Movie.Id.ShouldBe(review.Movie.Id);
    }

    [Fact]
    public async Task Update_ShouldReturn_NotFoundError()
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
                    Id = Guid.NewGuid(),
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
        result.Data.UpdateReview.ShouldBeNull();

        result.Errors.ShouldNotBeNull().Count.ShouldBe(1);
        result.Errors[0].Extensions.Code.ShouldBe("NOT_FOUND");
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
        result.Data.UpdateReview.ShouldBeNull();

        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
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
        result.Errors[0].Message.ShouldBe("Variable `review` is required.");
    }

    #endregion Update

    #region Paging

    [Fact]
    public async Task Reviews_ShouldFilter_ByMovie()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={reviews(first:50){nodes{movie{id}}}}");
        var movieId = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews.Nodes[0].Movie.Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{reviews(movieId:\"{movieId}\",first:50){{totalCount,nodes{{id,stars,movie{{id}}}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Errors.ShouldBeNull();

        var page = result.Data.Reviews;
        page.TotalCount.ShouldBeGreaterThan(0);
        page.Nodes.ShouldAllBe(r => r.Movie.Id == movieId);
    }

    [Fact]
    public async Task Reviews_ShouldFilter_ByAuthor()
    {
        // Arrange
        using var client = Application.CreateClient();
        using var setupResponse = await client.GetAsync("/graphql?query={reviews(first:50){nodes{author{id}}}}");
        var authorId = (await setupResponse.Content.ReadAsStringAsync()).Deserialize<GraphData>().Data.Reviews.Nodes[0].Author.Id;

        // Act
        using var response = await client.GetAsync($"/graphql?query={{reviews(authorId:\"{authorId}\",first:50){{totalCount,nodes{{id,author{{id}}}}}}}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var page = result.Data.Reviews;
        page.TotalCount.ShouldBeGreaterThan(0);
        page.Nodes.ShouldAllBe(r => r.Author.Id == authorId);
    }

    [Fact]
    public async Task Reviews_ShouldReturn_ValidationError_ForInvalidCursor()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={reviews(after:\"not-a-cursor\"){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Reviews.ShouldBeNull();

        result.Errors.ShouldNotBeNull().Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("One or more validation errors occurred.");
        result.Errors[0].Extensions.Code.ShouldBe("VALIDATION");
        result.Errors[0].Extensions.Errors["After"].ShouldBe(["The cursor is not valid. Use the endCursor from a previous page, or leave it empty for the first page."]);
    }

    [Fact]
    public async Task Reviews_ShouldReturn_Error_WhenPageIsTooLarge()
    {
        // Arrange
        using var client = Application.CreateClient();

        // Act
        using var response = await client.GetAsync("/graphql?query={reviews(first:51){totalCount}}");
        var result = (await response.Content.ReadAsStringAsync()).Deserialize<GraphData>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        _ = result.ShouldNotBeNull();
        result.Data.Reviews.ShouldBeNull();

        result.Errors.ShouldNotBeNull().Count.ShouldBe(1);
        result.Errors[0].Extensions.Code.ShouldBe("HC0051");
    }

    #endregion Paging
}
