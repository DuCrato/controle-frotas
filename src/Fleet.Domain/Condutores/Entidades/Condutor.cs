using Fleet.Domain.Condutores.Enum;
using Fleet.Domain.Condutores.ValueObjects;

namespace Fleet.Domain.Condutores.Entidades
{
    public sealed class Condutor
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public string Cpf { get; private set; } = null!;
        public Cnh Cnh { get; private set; } = null!;
        public StatusCondutorEnum Status { get; private set; }

        private Condutor() { }

        public Condutor(
            string nome,
            string cpf,
            Cnh cnh,
            StatusCondutorEnum status)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do condutor é obrigatório.");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF do condutor é obrigatório.");

            if (cnh is null)
                throw new ArgumentNullException(nameof(cnh), "CNH é obrigatória.");

            if (!System.Enum.IsDefined(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Status inválido.");

            Id = Guid.NewGuid();
            Nome = nome.Trim();
            Cpf = cpf.Trim();
            Cnh = cnh;
            Status = status;
        }

        public void AtualizarStatus(StatusCondutorEnum novoStatus)
        {
            if (!System.Enum.IsDefined(novoStatus))
                throw new ArgumentOutOfRangeException(nameof(novoStatus), "Status inválido.");

            Status = novoStatus;
        }

        public bool PodeSerAlocado() => Status == StatusCondutorEnum.Ativo && !Cnh.EstaVencida();
        public void Suspender() => AtualizarStatus(StatusCondutorEnum.Suspenso);
        public void Reativar() 
        {
            if (Cnh.EstaVencida())
                throw new InvalidOperationException("Não é possível reativar um condutor com CNH vencida.");

            AtualizarStatus(StatusCondutorEnum.Ativo);
        } 
    }
}
