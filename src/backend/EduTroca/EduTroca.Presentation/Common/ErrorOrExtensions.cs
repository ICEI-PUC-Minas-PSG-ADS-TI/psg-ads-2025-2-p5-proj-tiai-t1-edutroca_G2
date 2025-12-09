using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduTroca.Presentation.Common;

public static class ErrorOrExtensions
{
    public static IActionResult ToActionResult<T>(this ErrorOr<T> result)
    {
        return result.Match(
            value => HandleSuccess(value),
            errors => HandleErrors(errors)
        );
    }
    public static IActionResult ToActionResult<TResult, TResponse>(this ErrorOr<TResult> result, Func<TResult, TResponse> mapper)
    {
        return result.Match(
            value => HandleSuccess(mapper(value)),
            errors => HandleErrors(errors)
        );
    }

    private static IActionResult HandleSuccess<T>(T value)
    {
        if (value is Success)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(value);
    }

    private static IActionResult HandleErrors(List<Error> errors)
    {
        if (errors.Count > 0 && errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationErrors(errors);
        }

        var firstError = errors[0];

        return Problem(firstError);
    }

    private static IActionResult ValidationErrors(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return new BadRequestObjectResult(new ValidationProblemDetails(modelStateDictionary)
        {
            Title = "Erros de Validação",
            Detail = "Um ou mais erros de validação ocorreram na requisição.",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }

    private static IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(error.Type),
            Detail = error.Description,
            Type = error.Code
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Requisição Inválida",
        ErrorType.Conflict => "Conflito de Recursos",
        ErrorType.NotFound => "Recurso Não Encontrado",
        ErrorType.Unauthorized => "Não Autorizado",
        ErrorType.Forbidden => "Acesso Proibido",
        _ => "Erro Interno do Servidor"
    };
}