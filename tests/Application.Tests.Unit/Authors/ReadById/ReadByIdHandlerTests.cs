namespace CleanGraphQLApi.Application.Tests.Unit.Authors.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Authors.ReadById;
using CleanGraphQLApi.Application.Common.Interfaces;
using NSubstitute;
using Xunit;

public class ReadByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAuthorById(query.Id, token);
    }
}
