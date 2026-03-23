using Fleet.Application.Viagens.Interface;
using Fleet.Domain.Viagens.Entidades;
using Fleet.Domain.Viagens.Enum;
using Fleet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

public sealed class ViagemRepository(FleetDbContext context) : IViagemRepository
{
    public async Task CriarAsync(Viagem viagem, CancellationToken cancellationToken)
    {
        await context.Viagens.AddAsync(viagem, cancellationToken);
    }

    public async Task<Viagem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Viagens.FindAsync([id], cancellationToken);
    }

    public async Task<List<Viagem>> ListagemAsync(CancellationToken cancellationToken)
    {
        return await context.Viagens.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<Viagem>> ListagemPorCondutorAsync(Guid condutorId, CancellationToken cancellationToken)
    {
        return await context.Viagens
            .Where(v => v.CondutorId == condutorId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Viagem>> ListagemPorVeiculoAsync(Guid veiculoId, CancellationToken cancellationToken)
    {
        return await context.Viagens
            .Where(v => v.VeiculoId == veiculoId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CondutorTemViagemEmAndamentoAsync(Guid condutorId, CancellationToken cancellationToken)
    {
        return await context.Viagens
            .AnyAsync(v => v.CondutorId == condutorId && v.Status == StatusViagemEnum.EmAndamento, cancellationToken);
    }

    public async Task<bool> VeiculoTemViagemEmAndamentoAsync(Guid veiculoId, CancellationToken cancellationToken)
    {
        return await context.Viagens
            .AnyAsync(v => v.VeiculoId == veiculoId && v.Status == StatusViagemEnum.EmAndamento, cancellationToken);
    }

    public void Deletar(Viagem viagem)
    {
        context.Viagens.Remove(viagem);
    }

    public async Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
