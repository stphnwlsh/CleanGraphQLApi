namespace CleanGraphQL.Presentation.MovieReviews;

using System;
using Authors = Application.Authors;
using Movies = Application.Movies;
using Reviews = Application.Reviews;
using CleanGraphQL.Domain.Authors.Entities;
using CleanGraphQL.Domain.Movies.Entities;
using CleanGraphQL.Domain.Reviews.Entities;
using CleanGraphQL.Presentation.MovieReviews.Types.Objects;
using GraphQL;
using GraphQL.Types;
using MediatR;

public class MovieReviewQueries : ObjectGraphType<object>
{
    public MovieReviewQueries(IMediator mediator)
    {
        this.Name = "MovieReviewQueries";
        this.Description = "The base query for all the movie reviews in our object graph.";


        #region Authors

        _ = this.FieldAsync<ListGraphType<AuthorObject>>(
            name: "authors",
            description: "Gets a list of all authors.",
            resolve: async context => await mediator.Send(new Authors.ReadAll.ReadAllQuery()));

        _ = this.FieldAsync<AuthorObject, Author>(
            name: "author",
            description: "Gets an author by their unique identifier.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the author."
                }),
            resolve: async context => await mediator.Send(new Authors.ReadById.ReadByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

        #endregion Authors

        #region Movies

        _ = this.FieldAsync<ListGraphType<MovieObject>>(
            name: "movies",
            description: "Gets a list of all movies.",
            resolve: async context => await mediator.Send(new Movies.ReadAll.ReadAllQuery()));

        _ = this.FieldAsync<MovieObject, Movie>(
            name: "movie",
            description: "Gets a movie by its unique identifier.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the movie."
                }),
            resolve: async context => await mediator.Send(new Movies.ReadById.ReadByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

        #endregion Movies

        #region Reviews

        _ = this.FieldAsync<ListGraphType<ReviewObject>>(
            name: "reviews",
            description: "Gets a list of all reviews.",
            resolve: async context => await mediator.Send(new Reviews.ReadAll.ReadAllQuery()));

        _ = this.FieldAsync<ReviewObject, Review>(
            name: "review",
            description: "Gets a review by its unique identifier.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the review."
                }),
            resolve: async (context) => await mediator.Send(new Reviews.ReadById.ReadByIdQuery { Id = context.GetArgument("id", Guid.Empty) }));

        #endregion Reviews
    }
}
