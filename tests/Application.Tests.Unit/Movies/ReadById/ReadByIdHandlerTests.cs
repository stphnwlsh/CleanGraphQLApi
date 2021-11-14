namespace CleanGraphQLApi.Application.Tests.Unit.Movies.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Application.Movies.ReadById;
using NSubstitute;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IMoviesRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadMovieById(query.Id, token);
    }
}
