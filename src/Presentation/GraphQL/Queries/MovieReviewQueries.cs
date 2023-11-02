// namespace CleanGraphQLApi.Presentation.GraphQL.Queries;

// using System;
// using Application.Authors.Entities;
// using Application.Movies.Entities;
// using Application.Reviews.Entities;
// using CleanGraphQLApi.Presentation.GraphQL.Types;
// using global::GraphQL;
// using global::GraphQL.Types;
// using MediatR;
// using Authors = Application.Authors;
// using Movies = Application.Movies;
// using Reviews = Application.Reviews;

// public class MovieReviewQueries : ObjectGraphType<object>
// {
//     public MovieReviewQueries(IMediator mediator)
//     {
//         this.Name = "MovieReviewQueries";
//         this.Description = "The base query for all the movie reviews in our object graph.";


//         #region Authors

//         _ = this.FieldAsync<AuthorType, Author>(
//             name: "author",
//             description: "Gets an author by their unique identifier.",
//             arguments: new QueryArguments(
//                 new QueryArgument<NonNullGraphType<IdGraphType>>
//                 {
//                     Name = "id",
//                     Description = "The unique GUID of the author."
//                 }),
//             resolve: async context => await mediator.Send(new Authors.Queries.GetAuthorById.GetAuthorByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

//         #endregion Authors

//         #region Movies

//         _ = this.FieldAsync<MovieType, Movie>(
//             name: "movie",
//             description: "Gets a movie by its unique identifier.",
//             arguments: new QueryArguments(
//                 new QueryArgument<NonNullGraphType<IdGraphType>>
//                 {
//                     Name = "id",
//                     Description = "The unique GUID of the movie."
//                 }),
//             resolve: async context => await mediator.Send(new Movies.Queries.GetMovieById.GetMovieByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

//         #endregion Movies

//         #region Reviews

//         _ = this.FieldAsync<ReviewType, Review>(
//             name: "review",
//             description: "Gets a review by its unique identifier.",
//             arguments: new QueryArguments(
//                 new QueryArgument<NonNullGraphType<IdGraphType>>
//                 {
//                     Name = "id",
//                     Description = "The unique GUID of the review."
//                 }),
//             resolve: async (context) => await mediator.Send(new Reviews.Queries.GetReviewById.GetReviewByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

//         #endregion Reviews
//     }
// }
