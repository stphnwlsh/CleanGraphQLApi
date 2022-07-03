namespace CleanGraphQLApi.Presentation.GraphQL.Authors.Queries;

//using CleanGraphQLApi.Application.Authors.Entities;
using Types;


public class AuthorQueries
{

    public List<Book> GetBooks()
    {
        return new List<Book> { new Book { Title = "C# in depth", Author = new Author { Name = "Jon Skeet" } } };
    }

    public Book GetBook()
    {
        return new Book { Title = "C# in depth", Author = new Author { Name = "Jon Skeet" } };
    }
}

// public class AuthorQueries
// {

//     [UsePaging]
//     public async Task<List<Author>> GetAuthors(
//         [Service] IMediator mediator,
//         CancellationToken cancellationToken)
//     {
//         return await mediator.Send(new Queries.GetAuthors.GetAuthorsQuery(), cancellationToken);
//     }

//     // [GraphQLName("author")]
//     // [GraphQLDescription("Lookup all Authors")]
//     public async Task<Author> GetAuthorsById(
//         Guid id,
//         [Service] IMediator mediator,
//         [Service] IMapper mapper,
//         CancellationToken cancellationToken)
//     {
//         var result = await mediator.Send(new Queries.GetAuthorById.GetAuthorByIdQuery { Id = id }, cancellationToken);

//         return mapper.Map<Author>(result);
//     }
// }
