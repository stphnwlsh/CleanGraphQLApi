// namespace CleanGraphQLApi.Presentation.GraphQL.InputTypes;

// public sealed class CreateReviewInputType : InputObjectType<CreateReview>
// {
//     protected override void Configure(IObjectTypeDescriptor<CreateReview> descriptor)
//     {
//         descriptor.Name("Create Review");
//         descriptor.Description = "A review of a movie to be created in the collection";

//         _ = descriptor.Field("AuthorId").Description("Id of the Author of the review.");
//         _ = descriptor.Field<IdGraphType>("MovieId", "Id of the Movie being reviewed.");
//         _ = descriptor.Field<IntGraphType>("Stars", "Star rating out of five.");
//     }
// }
