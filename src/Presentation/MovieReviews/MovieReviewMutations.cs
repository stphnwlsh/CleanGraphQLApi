namespace CleanGraphQLApi.Presentation.MovieReviews;

using System;
using CleanGraphQLApi.Application.Reviews.Create;
using CleanGraphQLApi.Application.Reviews.Delete;
using CleanGraphQLApi.Domain.Reviews.Entities;
using CleanGraphQLApi.Presentation.MovieReviews.Types.InputObjects;
using CleanGraphQLApi.Presentation.MovieReviews.Types.Objects;
using GraphQL;
using GraphQL.Types;
using MediatR;

public class MovieReviewMutations : ObjectGraphType<object>
{
    public MovieReviewMutations(IMediator mediator)
    {
        this.Name = "MovieReviewMutations";
        this.Description = "The base mutation for all the entities in our object graph.";

        _ = this.FieldAsync<ReviewObject, Review>(
            "createReview",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ReviewInputType>> { Name = "review" }),
            resolve: async (context) =>
            {
                try
                {
                    return await mediator.Send(context.GetArgument<CreateCommand>("review"));
                }
                catch (Exception ex)
                {
                    context.Errors.Add(new ExecutionError(ex.Message, ex));
                    return null;
                }
            }
        );

        _ = this.FieldAsync<BooleanGraphType>(
            "deleteReview",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
            resolve: async (context) =>
            {
                try
                {
                    return await mediator.Send(new DeleteCommand { Id = context.GetArgument<Guid>("id") });
                }
                catch (Exception ex)
                {
                    context.Errors.Add(new ExecutionError($"A an error occured while attempting to delete the review.", ex));
                    return false;
                }
            });
    }
}
