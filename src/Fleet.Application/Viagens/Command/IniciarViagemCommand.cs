using MediatR;

namespace Fleet.Application.Viagens.Command;

public sealed record IniciarViagemCommand(
    Guid ViagemId,
    decimal QuiliometragemInicial
) : IRequest;
