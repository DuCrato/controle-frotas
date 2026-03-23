using Fleet.Application.Viagens.Command;
using Fleet.Application.Viagens.Interface;
using Fleet.Domain.Viagens.Entidades;
using Fleet.Domain.Viagens.ValueObjects;
using MediatR;

namespace Fleet.Application.Viagens.Handler;

public sealed class CriarViagemHandler(IViagemRepository repository) : IRequestHandler<CriarViagemCommand, Guid>
{
    public async Task<Guid> Handle(CriarViagemCommand request, CancellationToken cancellationToken)
    {
        var condutorTemViagemEmAndamento = 
            await repository.CondutorTemViagemEmAndamentoAsync(request.CondutorId, cancellationToken);

        if (condutorTemViagemEmAndamento)
            throw new InvalidOperationException("Condutor possui uma viagem em andamento. Conclua-a antes de iniciar uma nova.");

        var veiculoTemViagemEmAndamento = 
            await repository.VeiculoTemViagemEmAndamentoAsync(request.VeiculoId, cancellationToken);

        if (veiculoTemViagemEmAndamento)
            throw new InvalidOperationException("Veículo possui uma viagem em andamento. Conclua-a antes de iniciar uma nova.");

        var origem = new Localizacao(request.LatitudeOrigem, request.LongitudeOrigem, request.EnderecoOrigem);
        var destino = new Localizacao(request.LatitudeDestino, request.LongitudeDestino, request.EnderecoDestino);

        var viagem = new Viagem(
            request.VeiculoId,
            request.CondutorId,
            origem,
            destino,
            request.DataHoraPrevistaSaida,
            request.DataHoraPrevistaChegada,
            request.DistanciaEstimada,
            request.Observacoes);

        await repository.CriarAsync(viagem, cancellationToken);
        await repository.SalvarAlteracoesAsync(cancellationToken);

        return viagem.Id;
    }
}
