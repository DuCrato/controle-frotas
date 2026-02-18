using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.ValueObjects;
using MediatR;

namespace Fleet.Application.Veiculos.Handler;

public sealed class AtualizarVeiculoHandler(IVeiculoRepository repository) : IRequestHandler<AtualizarVeiculoCommand>
{
    public async Task Handle(AtualizarVeiculoCommand request, CancellationToken cancellationToken)
    {
        var veiculo = await repository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Veículo não encontrado.");

        veiculo.AtualizarProprietario(request.NomeProprietario);
        veiculo.AlterarStatus(request.Status);
        veiculo.AtualizarEndereco(new Endereco(request.Estado, request.Cidade));

        await repository.SalvarAlteracoesAsync(cancellationToken);

    }
}