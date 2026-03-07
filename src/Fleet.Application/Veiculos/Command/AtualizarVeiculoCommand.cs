using Fleet.Domain.Veiculos.Enum;
using MediatR;

namespace Fleet.Application.Veiculos.Command;

public sealed record AtualizarVeiculoCommand(
    Guid Id,
    string NomeProprietario,
    StatusVeiculoEnum Status,
    string Estado,
    string Cidade
) : IRequest;