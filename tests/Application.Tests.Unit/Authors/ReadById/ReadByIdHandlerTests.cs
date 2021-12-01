namespace CleanGraphQLApi.Application.Tests.Unit.Authors.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanGraphQLApi.Application.Authors.ReadById;
using CleanGraphQLApi.Application.Common.Exceptions;
using CleanGraphQLApi.Application.Common.Interfaces;
using CleanGraphQLApi.Application.Entities;
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

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.ReadAuthorById(Arg.Any<Guid>(), token).Returns(new Author
        {
            Id = Guid.Empty,
            FirstName = "FirstName",
            LastName = "LastName"
        });

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).ReadAuthorById(query.Id, token);

        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(Guid.Empty);
        result.FirstName.ShouldBe("FirstName");
        result.LastName.ShouldBe("LastName");
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new ReadByIdQuery { Id = Guid.Empty };

        var context = Substitute.For<IAuthorsRepository>();
        var handler = new ReadByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).ReadAuthorById(query.Id, token);

        exception.Message.ShouldBe("The Author with the supplied id was not found.");
    }
}
