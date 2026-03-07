using Fleet.Domain.Condutores.Entidades;

namespace Fleet.Application.Condutores.Interface
{
    public interface ICondutorRepository
    {
        Task<Condutor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Condutor?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<bool> ExisteCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<List<Condutor>> ListagemAsync(CancellationToken cancellationToken);

        Task CriarAsync(Condutor condutor, CancellationToken cancellationToken);
        void Deletar(Condutor condutor);
        Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);

    }
}
