using Fleet.Domain.Veiculos.Entidades;

namespace Fleet.Application.Veiculos.Interface
{
    public interface IVeiculoRepository
    {
        Task<bool> ExistePorPlacaAsync(string placaNormalizada, CancellationToken cancellationToken);
        Task CriarAsync(Veiculo veiculo, CancellationToken cancellationToken);

        Task<Veiculo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Veiculo>> ListagemAsync(CancellationToken cancellationToken);

        void Delete(Veiculo veiculo);
        Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
    }
}
