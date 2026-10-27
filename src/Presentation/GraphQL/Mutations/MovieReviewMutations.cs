namespace CleanGraphQLApi.Presentation.GraphQL.Mutations;

using System;
using CleanGraphQLApi.Presentation.GraphQL.Extensions;
using CleanGraphQLApi.Presentation.GraphQL.InputTypes;
using CleanGraphQLApi.Presentation.GraphQL.Types;
using global::GraphQL;
using global::GraphQL.Types;
using Commands = Application.Reviews.Commands;
using Entities = Application.Reviews.Entities;

public class MovieReviewMutations : ObjectGraphType<object>
{
    public MovieReviewMutations()
    {
        this.Name = "MovieReviewMutations";
        this.Description = "The base mutation for all the entities in our object graph.";

        _ = this.FieldAsync<ReviewType, Entities.Review>(
            "createReview",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CreateReviewInputType>> { Name = "input" }),
            resolve: async context => await context.Sender().Send(context.GetArgument<Commands.CreateReview.CreateReviewCommand>("input")));

        _ = this.FieldAsync<BooleanGraphType>(
            "deleteReview",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
            resolve: async context => await context.Sender().Send(new Commands.DeleteReview.DeleteReviewCommand { Id = context.GetArgument<Guid>("id") }));

        _ = this.FieldAsync<BooleanGraphType>(
            "updateReview",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateReviewInputType>> { Name = "input" }),
            resolve: async context => await context.Sender().Send(context.GetArgument<Commands.UpdateReview.UpdateReviewCommand>("input")));
    }
}
