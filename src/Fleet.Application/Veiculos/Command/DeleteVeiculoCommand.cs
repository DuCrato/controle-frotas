using MediatR;

namespace Fleet.Application.Veiculos.Command
{
    public sealed record DeleteVeiculoCommand(Guid Id) : IRequest;
}
