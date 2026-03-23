using MediatR;

namespace Fleet.Application.Viagens.Command;

public sealed record ConcluirViagemCommand(
    Guid ViagemId,
    decimal QuiliometragemFinal
) : IRequest;
