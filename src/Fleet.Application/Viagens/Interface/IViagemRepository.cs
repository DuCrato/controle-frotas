using Fleet.Domain.Viagens.Entidades;

namespace Fleet.Application.Viagens.Interface;

public interface IViagemRepository
{
    Task CriarAsync(Viagem viagem, CancellationToken cancellationToken);
    Task<Viagem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Viagem>> ListagemAsync(CancellationToken cancellationToken);
    Task<List<Viagem>> ListagemPorCondutorAsync(Guid condutorId, CancellationToken cancellationToken);
    Task<List<Viagem>> ListagemPorVeiculoAsync(Guid veiculoId, CancellationToken cancellationToken);
    Task<bool> CondutorTemViagemEmAndamentoAsync(Guid condutorId, CancellationToken cancellationToken);
    Task<bool> VeiculoTemViagemEmAndamentoAsync(Guid veiculoId, CancellationToken cancellationToken);
    void Deletar(Viagem viagem);
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
