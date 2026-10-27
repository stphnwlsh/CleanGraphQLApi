namespace CleanGraphQLApi.Presentation.GraphQL.Errors;

using Application.Common.Exceptions;
using FluentValidation;
using global::GraphQL;
using global::GraphQL.Execution;

/// <summary>
/// Maps exceptions raised by the Application layer to GraphQL errors.
/// </summary>
public class ApplicationErrorInfoProvider : ErrorInfoProvider
{
    public override ErrorInfo GetInfo(ExecutionError executionError)
    {
        var info = base.GetInfo(executionError);

        switch (executionError.InnerException)
        {
            case ValidationException validationException:
                info.Message = "One or more validation errors occurred.";
                info.Extensions ??= new Dictionary<string, object>();
                info.Extensions["errors"] = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray());
                break;
            case NotFoundException notFoundException:
                info.Message = notFoundException.Message;
                break;
        }

        return info;
    }
}
