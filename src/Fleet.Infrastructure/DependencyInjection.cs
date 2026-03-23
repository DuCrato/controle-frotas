using Fleet.Application.Condutores.Interface;
using Fleet.Application.Veiculos.Interface;
using Fleet.Application.Viagens.Interface;
using Fleet.Infrastructure.Context;
using Fleet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FleetDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<ICondutorRepository, CondutorRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IViagemRepository, ViagemRepository>();

        return services;
    }
}
