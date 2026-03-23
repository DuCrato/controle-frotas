using MediatR;

namespace Fleet.Application.Viagens.Command;

public sealed record CriarViagemCommand(
    Guid VeiculoId,
    Guid CondutorId,
    decimal LatitudeOrigem,
    decimal LongitudeOrigem,
    string EnderecoOrigem,
    decimal LatitudeDestino,
    decimal LongitudeDestino,
    string EnderecoDestino,
    DateTime DataHoraPrevistaSaida,
    DateTime DataHoraPrevistaChegada,
    decimal DistanciaEstimada,
    string? Observacoes = null
) : IRequest<Guid>;
