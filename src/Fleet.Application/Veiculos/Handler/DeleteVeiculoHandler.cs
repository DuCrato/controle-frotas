using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Interface;
using MediatR;

namespace Fleet.Application.Veiculos.Handler;

public sealed class DeleteVeiculoHandler(IVeiculoRepository repository) : IRequestHandler<DeleteVeiculoCommand>
{
    public async Task<Unit> Handle(DeleteVeiculoCommand request, CancellationToken cancellationToken)
    {
        var veiculo = await repository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");

        repository.Delete(veiculo);

        await repository.SalvarAlteracoesAsync(cancellationToken);

        return Unit.Value;
    }
}