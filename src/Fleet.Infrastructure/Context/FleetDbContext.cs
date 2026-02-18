using Fleet.Domain.Veiculos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Context;

public sealed class FleetDbContext(DbContextOptions<FleetDbContext> options) : DbContext(options)
{
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetDbContext).Assembly);
    
}