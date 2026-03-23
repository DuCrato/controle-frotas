using MediatR;

namespace Fleet.Application.Viagens.Query;
public sealed record ListagemViagensPorCondutorQuery(Guid CondutorId) : IRequest<List<ListagemViagensQueryResponse>>;
public sealed record ListagemViagensPorVeiculoQuery(Guid VeiculoId) : IRequest<List<ListagemViagensQueryResponse>>;
