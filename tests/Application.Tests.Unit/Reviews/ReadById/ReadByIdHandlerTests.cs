namespace CleanGraphQLApi.Application.Tests.Unit.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Application.Reviews.ReadById;
using CleanGraphQLApi.Domain.Reviews.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IReviewsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadReviewById(Arg.Any<Guid>(), token).Returns(new Review
        {
            Id = Guid.Empty,
            Stars = 5,
            ReviewAuthorId = Guid.Empty,
            ReviewedMovieId = Guid.Empty
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);

        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.Stars.ShouldBe(5);
        result.ReviewAuthorId.ShouldBe(Guid.Empty);
        result.ReviewedMovieId.ShouldBe(Guid.Empty);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IReviewsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);

        exception.Message.ShouldBe("The Review with the supplied id was not found.");
    }
}
