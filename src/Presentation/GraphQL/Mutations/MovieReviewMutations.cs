// namespace CleanGraphQLApi.Presentation.GraphQL.Mutations;

// using System;
// using CleanGraphQLApi.Presentation.GraphQL.InputTypes;
// using CleanGraphQLApi.Presentation.GraphQL.Types;
// using global::GraphQL;
// using global::GraphQL.Types;
// using MediatR;
// using Commands = Application.Reviews.Commands;
// using Entities = Application.Reviews.Entities;

// public class MovieReviewMutations : ObjectGraphType<object>
// {
//     public MovieReviewMutations(IMediator mediator)
//     {
//         this.Name = "MovieReviewMutations";
//         this.Description = "The base mutation for all the entities in our object graph.";

//         _ = this.FieldAsync<ReviewType, Entities.Review>(
//             "createReview",
//             arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CreateReviewInputType>> { Name = "input" }),
//             resolve: async (context) =>
//             {
//                 try
//                 {
//                     var input = context.GetArgument<Commands.CreateReview.CreateReviewCommand>("input");
//                     ArgumentNullException.ThrowIfNull(input);

//                     return await mediator.Send(input);
//                 }
//                 catch (Exception ex)
//                 {
//                     context.Errors.Add(new ExecutionError("A an error occured while attempting to create the review.", ex));
//                     return null;
//                 }
//             }
//         );

//         _ = this.FieldAsync<BooleanGraphType>(
//             "deleteReview",
//             arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
//             resolve: async (context) =>
//             {
//                 try
//                 {
//                     var input = context.GetArgument<Guid>("id");

//                     ArgumentNullException.ThrowIfNull(input);

//                     return await mediator.Send(new Commands.DeleteReview.DeleteReviewCommand { Id = input });
//                 }
//                 catch (Exception ex)
//                 {
//                     context.Errors.Add(new ExecutionError($"A an error occured while attempting to delete the review.", ex));
//                     return false;
//                 }
//             });

//         _ = this.FieldAsync<BooleanGraphType>(
//             "updateReview",
//             arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateReviewInputType>> { Name = "input" }),
//             resolve: async (context) =>
//             {
//                 try
//                 {
//                     var input = context.GetArgument<Commands.UpdateReview.UpdateReviewCommand>("input");

//                     ArgumentNullException.ThrowIfNull(input);

//                     return await mediator.Send(input);
//                 }
//                 catch (Exception ex)
//                 {
//                     context.Errors.Add(new ExecutionError("A an error occured while attempting to update the review.", ex));
//                     return false;
//                 }
//             }
//         );
//     }
// }
