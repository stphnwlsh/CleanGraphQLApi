namespace CleanGraphQL.Application.Tests.Unit.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQL.Application.Common.Interfaces;
using CleanGraphQL.Application.Reviews.ReadById;
using NSubstitute;
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

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadReviewById(query.Id, token);
    }
}
