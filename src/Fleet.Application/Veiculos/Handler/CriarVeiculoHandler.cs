using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Entidades;
using Fleet.Domain.Veiculos.ValueObjects;
using MediatR;

namespace Fleet.Application.Veiculos.Handler
{
    public sealed class CriarVeiculoHandler(IVeiculoRepository repository) : IRequestHandler<CriarVeiculoCommand, Guid>
    {
        public async Task<Guid> Handle(CriarVeiculoCommand request, CancellationToken cancellationToken)
        {
            var placa = new Placa(request.Placa);
            var renavam = new Renavam(request.Renavam);
            var chassi = new Chassi(request.Chassi);
            var endereco = new Endereco(request.Estado, request.Cidade);

            if (await repository.ExistePorPlacaAsync(placa.Valor, cancellationToken))
                throw new InvalidOperationException($"Já existe um veículo cadastrado com está placa: {placa.Valor}.");

            var veiculo = new Veiculo(
                placa,
                renavam,
                chassi,
                request.NomeProprietario,
                request.Status,
                endereco);

            await repository.CriarAsync(veiculo, cancellationToken);
            await repository.SalvarAlteracoesAsync(cancellationToken);

            return veiculo.Id;
        }
    }
}
