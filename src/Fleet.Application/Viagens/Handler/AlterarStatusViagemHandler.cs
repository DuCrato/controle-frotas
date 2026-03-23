using Fleet.Application.Viagens.Command;
using Fleet.Application.Viagens.Interface;
using MediatR;

namespace Fleet.Application.Viagens.Handler;

public sealed class IniciarViagemHandler(IViagemRepository repository) : IRequestHandler<IniciarViagemCommand>
{
    public async Task Handle(IniciarViagemCommand request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Nenhuma viagem encontrada com o ID: {request.ViagemId}");

        viagem.Iniciar(request.QuiliometragemInicial);

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}

public sealed class ConcluirViagemHandler(IViagemRepository repository) : IRequestHandler<ConcluirViagemCommand>
{
    public async Task Handle(ConcluirViagemCommand request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Nenhuma viagem encontrada com o ID: {request.ViagemId}");

        viagem.Concluir(request.QuiliometragemFinal);

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}

public sealed class PausarViagemHandler(IViagemRepository repository) : IRequestHandler<PausarViagemCommand>
{
    public async Task Handle(PausarViagemCommand request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Nenhuma viagem encontrada com o ID: {request.ViagemId}");

        viagem.Pausar();

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}

public sealed class RetomarViagemHandler(IViagemRepository repository) : IRequestHandler<RetomarViagemCommand>
{
    public async Task Handle(RetomarViagemCommand request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Nenhuma viagem encontrada com o ID: {request.ViagemId}");

        viagem.Retomar();

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}

public sealed class CancelarViagemHandler(IViagemRepository repository) : IRequestHandler<CancelarViagemCommand>
{
    public async Task Handle(CancelarViagemCommand request, CancellationToken cancellationToken)
    {
        var viagem = await repository.ObterPorIdAsync(request.ViagemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Nenhuma viagem encontrada com o ID: {request.ViagemId}");

        viagem.Cancelar();

        await repository.SalvarAlteracoesAsync(cancellationToken);
    }
}
