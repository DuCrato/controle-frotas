using Fleet.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // 1. Registra o MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // 2. Registra todos os validadores do FluentValidation automaticamente
        services.AddValidatorsFromAssembly(assembly); // <--- Faltava essa linha!

        // 3. Registra o Pipeline Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}