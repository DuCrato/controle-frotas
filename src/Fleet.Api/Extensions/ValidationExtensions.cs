using FluentValidation;

namespace Fleet.Api.Extensions;

/// <summary>
/// Extensões para registrar validadores do FluentValidation.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Adiciona validadores do FluentValidation ao container de DI.
    /// </summary>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Registrar validadores do assembly
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}
