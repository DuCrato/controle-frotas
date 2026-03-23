using Fleet.Application.Viagens.Interface;
using Fleet.Application.Viagens.Query;
using MediatR;

namespace Fleet.Application.Viagens.Handler;

public sealed class ObterViagemPorIdHandler(IViagemRepository repository) : IRequestHandler<ObterViagemPorIdQuery, ObterViagemPorIdQueryResponse?>
{
    public async Task<ObterViagemPorIdQueryResponse?> Handle(ObterViagemPorIdQuery request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken);

        if (viagem is null)
            return null;

        return MapearParaResponse(viagem);
    }

    private static ObterViagemPorIdQueryResponse MapearParaResponse(Fleet.Domain.Viagens.Entidades.Viagem viagem)
    {
        return new ObterViagemPorIdQueryResponse(
            viagem.Id,
            viagem.VeiculoId,
            viagem.CondutorId,
            viagem.Origem.ToString(),
            viagem.Destino.ToString(),
            viagem.DataHoraPrevistaSaida,
            viagem.DataHoraPrevistaChegada,
            viagem.DataHoraRealSaida,
            viagem.DataHoraRealChegada,
            viagem.QuiliometragemInicial,
            viagem.QuiliometragemFinal,
            viagem.DistanciaEstimada,
            viagem.Status.ToString(),
            viagem.Observacoes,
            viagem.DataCriacao);
    }
}

public sealed class ListagemViagensHandler(IViagemRepository repository) : IRequestHandler<ListagemViagensQuery, List<ListagemViagensQueryResponse>>
{
    public async Task<List<ListagemViagensQueryResponse>> Handle(ListagemViagensQuery request, CancellationToken cancellationToken)
    {
        var viagens = await repository.ListagemAsync(cancellationToken);

        return [.. viagens
            .Select(v => new ListagemViagensQueryResponse(
                v.Id,
                v.VeiculoId,
                v.CondutorId,
                v.Origem.ToString(),
                v.Destino.ToString(),
                v.DataHoraPrevistaSaida,
                v.DataHoraPrevistaChegada,
                v.Status.ToString(),
                v.DistanciaEstimada,
                v.DataCriacao))];
    }
}

public sealed class ListagemViagensPorCondutorHandler(IViagemRepository repository) : IRequestHandler<ListagemViagensPorCondutorQuery, List<ListagemViagensQueryResponse>>
{
    public async Task<List<ListagemViagensQueryResponse>> Handle(ListagemViagensPorCondutorQuery request, CancellationToken cancellationToken)
    {
        var viagens = await repository.ListagemPorCondutorAsync(request.CondutorId, cancellationToken);

        return [.. viagens
            .Select(v => new ListagemViagensQueryResponse(
                v.Id,
                v.VeiculoId,
                v.CondutorId,
                v.Origem.ToString(),
                v.Destino.ToString(),
                v.DataHoraPrevistaSaida,
                v.DataHoraPrevistaChegada,
                v.Status.ToString(),
                v.DistanciaEstimada,
                v.DataCriacao))];
    }
}

public sealed class ListagemViagensPorVeiculoHandler(IViagemRepository repository) : IRequestHandler<ListagemViagensPorVeiculoQuery, List<ListagemViagensQueryResponse>>
{
    public async Task<List<ListagemViagensQueryResponse>> Handle(ListagemViagensPorVeiculoQuery request, CancellationToken cancellationToken)
    {
        var viagens = await repository.ListagemPorVeiculoAsync(request.VeiculoId, cancellationToken);

        return [.. viagens
            .Select(v => new ListagemViagensQueryResponse(
                v.Id,
                v.VeiculoId,
                v.CondutorId,
                v.Origem.ToString(),
                v.Destino.ToString(),
                v.DataHoraPrevistaSaida,
                v.DataHoraPrevistaChegada,
                v.Status.ToString(),
                v.DistanciaEstimada,
                v.DataCriacao))];
    }
}
