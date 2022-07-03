namespace CleanGraphQLApi.Presentation.GraphQL.Authors.Types;

using CleanGraphQLApi.Application.Authors.Entities;


public class Book
{
    public string Title { get; set; }

    public Author Author { get; set; }
}

public class Author
{
    public string Name { get; set; }
}


public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        _ = descriptor
            .Field(f => f.Title)
            .Type<StringType>();

        _ = descriptor
            .Field(f => f.Author)
            .Type<StringType>();
    }
}

// [GraphQLName("author")]
// [GraphQLDescription("An Author of Movie Reviews")]
// public class AuthorType : ObjectType<Author>
// {

//     protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
//     {
//         _ = descriptor
//             .Field(f => f.Id)
//             .Name("id")
//             .Description("Identifier of the author")
//             .Type<NonNullType<IdType>>();
//     }

//     // public AuthorType()
//     // {
//     //     this.Description = "An Author of Movie Reviews";

//     //     _ = this.Field(m => m.Id);
//     //     _ = this.Field(m => m.FirstName).Description("First name of the movie reviewer");
//     //     _ = this.Field(m => m.LastName).Description("Last name of the movie reviewer");
//     //     _ = this.Field(
//     //         name: "reviews",
//     //         description: "This authors reviews",
//     //         type: typeof(ListGraphType<ReviewType>),
//     //         resolve: m => m.Source?.Reviews);
//     //     _ = this.Field(m => m.DateCreated).Description("Date the author was created");
//     //     _ = this.Field(m => m.DateModified).Description("Date the author was modified");
//     // }
// }
// // {
// //     [GraphQLName("id")]
// // [GraphQLDescription("Identifier of the Author")]
// // [GraphQLType(typeof(IdType))]
// // public Guid Id { get; init; }

// // [GraphQLName("firstName")]
// // [GraphQLDescription("First name of the Author")]
// // [GraphQLType(typeof(IdType))]
// // public string FirstName { get; init; }

// // [GraphQLName("lastName")]
// // [GraphQLDescription("Last name of the Author")]
// // [GraphQLType(typeof(StringType))]
// // public string LastName { get; init; }

// // [GraphQLName("reviews")]
// // [GraphQLDescription("Reviews the Author has written")]
// // [GraphQLType(typeof(ListType))]
// // public ICollection<Review> Reviews { get; init; }

// // [GraphQLName("dateCreated")]
// // [GraphQLDescription("Date the Author was created")]
// // [GraphQLType(typeof(DateTimeType))]
// // public DateTime DateCreated { get; init; }

// // [GraphQLName("dateModified")]
// // [GraphQLDescription("Date the Author was modified")]
// // [GraphQLType(typeof(DateTimeType))]
// // public DateTime DateModified { get; set; }
// // }
