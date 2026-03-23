using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fleet.Api.Filters;

/// <summary>
/// Filtro que valida automaticamente os modelos antes da ação do controller.
/// Retorna 400 BadRequest com erros de validação se houver problemas.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    private readonly IValidatorFactory _validatorFactory;

    public ValidationFilter(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Obter argumentos do método do controller
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validator = _validatorFactory.GetValidator(argument.GetType());

            if (validator is null)
                continue;

            var validationResult = await validator.ValidateAsync(
                new ValidationContext<object>(argument));

            if (!validationResult.IsValid)
                validationFailures.AddRange(validationResult.Errors);
        }

        if (validationFailures.Count > 0)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Instance = context.HttpContext.Request.Path
            };

            var errors = validationFailures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());

            problemDetails.Extensions["errors"] = errors;

            context.Result = new BadRequestObjectResult(problemDetails);
            return;
        }

        await next();
    }
}
