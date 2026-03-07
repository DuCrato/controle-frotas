using Fleet.Application.Condutores.Interface;
using Fleet.Domain.Condutores.Entidades;
using Fleet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Infrastructure.Repositories;

public sealed class CondutorRepository(FleetDbContext context) : ICondutorRepository
{
    public async Task<Condutor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Condutores.FindAsync([id], cancellationToken);
    }

    public async Task<Condutor?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        return await context.Condutores
            .FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);
    }

    public async Task<bool> ExisteCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        return await context.Condutores
            .AnyAsync(c => c.Cpf == cpf, cancellationToken);
    }

    public async Task<List<Condutor>> ListagemAsync(CancellationToken cancellationToken)
    {
        return await context.Condutores
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task CriarAsync(Condutor condutor, CancellationToken cancellationToken)
    {
        await context.Condutores.AddAsync(condutor, cancellationToken);
    }

    public void Deletar(Condutor condutor)
    {
        context.Condutores.Remove(condutor);
    }

    public async Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
