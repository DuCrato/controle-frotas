using Fleet.Domain.Veiculos.Enumeradores;
using MediatR;

namespace Fleet.Application.Veiculos.Command
{
    public sealed record CriarVeiculoCommand(
        string Placa,
        string Renavam,
        string Chassi,
        string NomeProprietario,
        StatusVeiculoEnum Status,
        string Estado,
        string Cidade) : IRequest<Guid>;
}
