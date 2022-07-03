namespace CleanGraphQLApi.Presentation.GraphQL.Authors.Types;

using CleanGraphQLApi.Presentation.GraphQL.Authors.Queries;
using MediatR;

public class QueryType : ObjectType<AuthorQueries>
{
    protected override void Configure(IObjectTypeDescriptor<AuthorQueries> descriptor)
    {

        _ = descriptor
            .Field(f => f.GetBooks())
            .Name("books")
            .Type<BookType>();

        _ = descriptor
            .Field(f => f.GetBook())
            .Name("book")
            .Type<BookType>();
    }
}

// public class AuthorQueriesType : ObjectType<AuthorQueries>
// {


//     protected override void Configure(IObjectTypeDescriptor<AuthorQueries> descriptor)
//     {
//         _ = descriptor
//             .Field(f => f.GetAuthors())
//             .Name("authors")
//             .Description("Lookup all Authors")
//             .Type<ListType<AuthorType>>();

//         _ = descriptor
//             .Field(f => f.GetAuthorsById(id: default))
//             .Name("author")
//             .Description("Lookup an Author by Id")
//             .Argument("id", arg => arg.Type<NonNullType<IdType>>())
//             .Type<AuthorType>();
//     }
// }
