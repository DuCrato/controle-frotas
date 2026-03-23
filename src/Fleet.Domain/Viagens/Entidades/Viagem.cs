using Fleet.Domain.Viagens.Enum;
using Fleet.Domain.Viagens.ValueObjects;

namespace Fleet.Domain.Viagens.Entidades;

/// <summary>
/// Entidade que representa uma viagem no sistema de controle de frotas.
/// Uma viagem é a alocação de um veículo com um condutor para ir de um ponto A a um ponto B.
/// </summary>
public sealed class Viagem
{
    public Guid Id { get; private set; }
    public Guid VeiculoId { get; private set; }
    public Guid CondutorId { get; private set; }
    public Localizacao Origem { get; private set; } = null!;
    public Localizacao Destino { get; private set; } = null!;
    public DateTime DataHoraPrevistaSaida { get; private set; }
    public DateTime DataHoraPrevistaChegada { get; private set; }
    public DateTime? DataHoraRealSaida { get; private set; }
    public DateTime? DataHoraRealChegada { get; private set; }
    public decimal? QuiliometragemInicial { get; private set; }
    public decimal? QuiliometragemFinal { get; private set; }
    public decimal DistanciaEstimada { get; private set; }
    public string? Observacoes { get; private set; }
    public StatusViagemEnum Status { get; private set; }
    public DateTime DataCriacao { get; private set; }

    private Viagem() { }

    public Viagem(
        Guid veiculoId,
        Guid condutorId,
        Localizacao origem,
        Localizacao destino,
        DateTime dataHoraPrevistaSaida,
        DateTime dataHoraPrevistaChegada,
        decimal distanciaEstimada,
        string? observacoes = null)
    {
        if (veiculoId == Guid.Empty)
            throw new ArgumentException("ID do veículo é obrigatório");

        if (condutorId == Guid.Empty)
            throw new ArgumentException("ID do condutor é obrigatório");

        ArgumentNullException.ThrowIfNull(origem);

        ArgumentNullException.ThrowIfNull(destino);

        if (dataHoraPrevistaSaida >= dataHoraPrevistaChegada)
            throw new ArgumentException("Data de saída deve ser anterior à data de chegada");

        if (dataHoraPrevistaSaida <= DateTime.Now)
            throw new ArgumentException("Data de saída deve ser no futuro");

        if (distanciaEstimada <= 0)
            throw new ArgumentException("Distância estimada deve ser maior que zero");

        Id = Guid.NewGuid();
        VeiculoId = veiculoId;
        CondutorId = condutorId;
        Origem = origem;
        Destino = destino;
        DataHoraPrevistaSaida = dataHoraPrevistaSaida;
        DataHoraPrevistaChegada = dataHoraPrevistaChegada;
        DistanciaEstimada = distanciaEstimada;
        Observacoes = observacoes?.Trim();
        Status = StatusViagemEnum.Planejada;
        DataCriacao = DateTime.UtcNow;
    }

    public void Iniciar(decimal quilometragemInicial)
    {
        if (Status != StatusViagemEnum.Planejada)
            throw new InvalidOperationException("Apenas viagens planejadas podem ser iniciadas");

        if (quilometragemInicial < 0)
            throw new ArgumentException("Quilometragem inicial não pode ser negativa");

        DataHoraRealSaida = DateTime.UtcNow;
        QuiliometragemInicial = quilometragemInicial;
        Status = StatusViagemEnum.EmAndamento;
    }

    public void Concluir(decimal quilometragemFinal)
    {
        if (Status != StatusViagemEnum.EmAndamento)
            throw new InvalidOperationException("Apenas viagens em andamento podem ser concluídas");

        if (quilometragemFinal < QuiliometragemInicial)
            throw new ArgumentException("Quilometragem final não pode ser menor que a inicial");

        DataHoraRealChegada = DateTime.UtcNow;
        QuiliometragemFinal = quilometragemFinal;
        Status = StatusViagemEnum.Concluida;
    }

    public void Pausar()
    {
        if (Status != StatusViagemEnum.EmAndamento)
            throw new InvalidOperationException("Apenas viagens em andamento podem ser pausadas");

        Status = StatusViagemEnum.Pausada;
    }

    public void Retomar()
    {
        if (Status != StatusViagemEnum.Pausada)
            throw new InvalidOperationException("Apenas viagens pausadas podem ser retomadas");

        Status = StatusViagemEnum.EmAndamento;
    }

    public void Cancelar()
    {
        if (Status == StatusViagemEnum.Concluida || Status == StatusViagemEnum.Cancelada)
            throw new InvalidOperationException("Viagens concluídas ou canceladas não podem ser modificadas");

        Status = StatusViagemEnum.Cancelada;
    }

    public decimal? ObterDistanciaPercorrida()
    {
        if (QuiliometragemInicial is null || QuiliometragemFinal is null)
            return null;

        return QuiliometragemFinal.Value - QuiliometragemInicial.Value;
    }

    public bool EstaAtrasada()
    {
        if (Status == StatusViagemEnum.EmAndamento)
            return DateTime.UtcNow > DataHoraPrevistaChegada;

        return false;
    }
}
