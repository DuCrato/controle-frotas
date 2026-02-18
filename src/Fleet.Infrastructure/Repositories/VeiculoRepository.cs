using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Entidades;
using Fleet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

public sealed class VeiculoRepository(FleetDbContext context) : IVeiculoRepository
{
    public async Task<bool> ExistePorPlacaAsync(string placaNormalizada, CancellationToken cancellationToken)
    {
        return await context.Veiculos
            .AnyAsync(v => v.Placa.Valor == placaNormalizada, cancellationToken);
    }

    public async Task CriarAsync(Veiculo veiculo, CancellationToken cancellationToken)
    {
        await context.Veiculos.AddAsync(veiculo, cancellationToken);
    }

    public async Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Veiculos.FindAsync([id], cancellationToken);
    }

    public async Task<List<Veiculo>> ListagemAsync(CancellationToken cancellationToken)
    {
        return await context.Veiculos.AsNoTracking().ToListAsync(cancellationToken);
    }

    public void Delete(Veiculo veiculo)
    {
        context.Veiculos.Remove(veiculo);
    }

    public async Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}