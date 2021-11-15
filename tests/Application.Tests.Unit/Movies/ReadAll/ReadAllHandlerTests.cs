namespace CleanGraphQLApi.Application.Tests.Unit.Movies.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Application.Movies.ReadAll;
using NSubstitute;
using Xunit;

public class ReadAllHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadAllQuery();

        var context = Substitute.For<IMoviesRepository>();
        var handler = new ReadAllHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAllMovies(token);
    }
}
