using Fleet.Domain.Condutores.Entidades;
using Fleet.Domain.Veiculos.Entidades;
using Fleet.Domain.Viagens.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Context;

public sealed class FleetDbContext(DbContextOptions<FleetDbContext> options) : DbContext(options)
{
    public DbSet<Condutor> Condutores => Set<Condutor>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Viagem> Viagens => Set<Viagem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetDbContext).Assembly);

}