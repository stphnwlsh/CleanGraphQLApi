namespace CleanGraphQLApi.Presentation.GraphQL.Errors;

using Application.Common.Exceptions;
using FluentValidation;
using HotChocolate;
using HotChocolate.Execution;

/// <summary>
/// Maps exceptions raised by the Application layer to GraphQL errors.
/// </summary>
public class ApplicationErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            ValidationException validationException => ErrorBuilder.FromError(error)
                .SetMessage("One or more validation errors occurred.")
                .SetCode("VALIDATION")
                .SetExtension("errors", validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, object? (g) => g.Select(e => e.ErrorMessage).Distinct().ToArray()))
                .Build(),
            NotFoundException notFoundException => ErrorBuilder.FromError(error)
                .SetMessage(notFoundException.Message)
                .SetCode("NOT_FOUND")
                .Build(),
            _ => error
        };
    }
}
