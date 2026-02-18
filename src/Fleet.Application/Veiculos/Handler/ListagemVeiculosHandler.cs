using Fleet.Application.Veiculos.Dtos;
using Fleet.Application.Veiculos.Interface;
using Fleet.Application.Veiculos.Query;
using MediatR;

namespace Fleet.Application.Veiculos.Handler;

public sealed class ListagemVeiculosHandler(IVeiculoRepository repository) : IRequestHandler<ListagemVeiculosQuery, List<VeiculoDto>>
{
    public async Task<List<VeiculoDto>> Handle(ListagemVeiculosQuery request, CancellationToken cancellationToken)
    {
        var veiculos = await repository.ListagemAsync(cancellationToken);

        return [.. veiculos
            .Select(v => new VeiculoDto(
                v.Id,
                v.Placa.Valor,
                v.Renavam.Valor,
                v.Chassi.Valor,
                v.NomeProprietario,
                v.Status.ToString(),
                v.Endereco.Estado,
                v.Endereco.Cidade
            ))];
    }
}