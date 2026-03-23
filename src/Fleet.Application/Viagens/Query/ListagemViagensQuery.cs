using MediatR;

namespace Fleet.Application.Viagens.Query;

public sealed record ListagemViagensQuery : IRequest<List<ListagemViagensQueryResponse>>;

public sealed record ListagemViagensQueryResponse(
    Guid Id,
    Guid VeiculoId,
    Guid CondutorId,
    string Origem,
    string Destino,
    DateTime DataHoraPrevistaSaida,
    DateTime DataHoraPrevistaChegada,
    string Status,
    decimal DistanciaEstimada,
    DateTime DataCriacao
);
