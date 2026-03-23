using MediatR;

namespace Fleet.Application.Viagens.Query;
public sealed record ObterViagemPorIdQuery(Guid ViagemId) : IRequest<ObterViagemPorIdQueryResponse?>;

public sealed record ObterViagemPorIdQueryResponse(
    Guid Id,
    Guid VeiculoId,
    Guid CondutorId,
    string Origem,
    string Destino,
    DateTime DataHoraPrevistaSaida,
    DateTime DataHoraPrevistaChegada,
    DateTime? DataHoraRealSaida,
    DateTime? DataHoraRealChegada,
    decimal? QuiliometragemInicial,
    decimal? QuiliometragemFinal,
    decimal DistanciaEstimada,
    string Status,
    string? Observacoes,
    DateTime DataCriacao
);
