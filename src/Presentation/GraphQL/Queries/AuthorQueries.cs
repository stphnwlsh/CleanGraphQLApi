namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

using Application.Authors.Entities;
using MediatR;
using Authors = Application.Authors;

public class AuthorQueries
{
    private readonly IMediator mediator;

    public AuthorQueries(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<List<Author>> GetAuthors()
    {
        return await this.mediator.Send(new Authors.Queries.GetAuthors.GetAuthorsQuery());
    }

    // public MovieReviewQueries(IMediator mediator)
    // {
    //     this.Name = "MovieReviewQueries";
    //     this.Description = "The base query for all the movie reviews in our object graph.";


    //     #region Authors



    //     // _ = this.FieldAsync<ListGraphType<AuthorType>>(
    //     //     name: "authors",
    //     //     description: "Gets a list of all authors.",
    //     //     resolve: async context =>

    //     // _ = this.FieldAsync<AuthorType, Author>(
    //     //     name: "author",
    //     //     description: "Gets an author by their unique identifier.",
    //     //     arguments: new QueryArguments(
    //     //         new QueryArgument<NonNullGraphType<IdGraphType>>
    //     //         {
    //     //             Name = "id",
    //     //             Description = "The unique GUID of the author."
    //     //         }),
    //     //     resolve: async context => await mediator.Send(new Authors.Queries.GetAuthorById.GetAuthorByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

    //         #endregion Authors
    // }
}
