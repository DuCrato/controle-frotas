using Fleet.Domain.Veiculos.Enumeradores;
using Fleet.Domain.Veiculos.ValueObjects;

namespace Fleet.Domain.Veiculos.Entidades
{
    public sealed class Veiculo
    {
        public Guid Id { get; private set; }

        public Placa Placa { get; private set; } = null!;
        public Renavam Renavam { get; private set; } = null!;
        public Chassi Chassi { get; private set; } = null!;

        public string NomeProprietario { get; private set; } = null!;
        public StatusVeiculoEnum Status { get; private set; }
        public Endereco Endereco { get; private set; } = null!;

        private Veiculo() { }

        public Veiculo(
            Placa placa,
            Renavam renavam,
            Chassi chassi,
            string nomeProprietario,
            StatusVeiculoEnum status,
            Endereco endereco)
        {
            if (string.IsNullOrWhiteSpace(nomeProprietario))
                throw new ArgumentException("Nome do proprietário é obrigatório.");

            if (!Enum.IsDefined(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Status inválido.");

            Id = Guid.NewGuid();
            Placa = placa ?? throw new ArgumentNullException(nameof(placa));
            Renavam = renavam ?? throw new ArgumentNullException(nameof(renavam));
            Chassi = chassi ?? throw new ArgumentNullException(nameof(chassi));
            Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
            NomeProprietario = nomeProprietario.Trim();
            Status = status;
        }

        public void AlterarStatus(StatusVeiculoEnum novoStatus)
        {
            if (!Enum.IsDefined(novoStatus))
                throw new ArgumentOutOfRangeException(nameof(novoStatus), "Status inválido.");

            Status = novoStatus;
        }

        public void AtualizarProprietario(string nomeProprietario)
        {
            if (string.IsNullOrWhiteSpace(nomeProprietario))
                throw new ArgumentException("Nome do proprietário é obrigatório.", nameof(nomeProprietario));

            NomeProprietario = nomeProprietario.Trim();
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
        }
    }
}
