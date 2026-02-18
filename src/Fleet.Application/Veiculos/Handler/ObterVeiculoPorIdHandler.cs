using Fleet.Application.Veiculos.Dtos;
using Fleet.Application.Veiculos.Interface;
using Fleet.Application.Veiculos.Query;
using MediatR;

namespace Fleet.Application.Veiculos.Handler;

public sealed class ObterVeiculoPorIdHandler(IVeiculoRepository repository) : IRequestHandler<ObterVeiculoPorIdQuery, VeiculoDto>
{
    public async Task<VeiculoDto> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
    {
        var veiculo = await repository.ObterPorIdAsync(request.Id, cancellationToken);

        return veiculo is null
            ? throw new KeyNotFoundException("Veículo não encontrado.")
            : new VeiculoDto(
                veiculo.Id,
                veiculo.Placa.Valor,
                veiculo.Renavam.Valor,
                veiculo.Chassi.Valor,
                veiculo.NomeProprietario,
                veiculo.Status.ToString(),
                veiculo.Endereco.Estado,
                veiculo.Endereco.Cidade
        );
    }
}