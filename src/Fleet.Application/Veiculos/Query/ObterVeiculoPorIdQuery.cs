using Fleet.Application.Veiculos.Dtos;
using MediatR;

namespace Fleet.Application.Veiculos.Query
{
    public sealed record ObterVeiculoPorIdQuery(Guid Id) : IRequest<VeiculoDto>;
}
