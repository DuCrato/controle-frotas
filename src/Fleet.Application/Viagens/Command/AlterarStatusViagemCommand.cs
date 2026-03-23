using MediatR;

namespace Fleet.Application.Viagens.Command;

public sealed record PausarViagemCommand(Guid ViagemId) : IRequest;
public sealed record RetomarViagemCommand(Guid ViagemId) : IRequest;
public sealed record CancelarViagemCommand(Guid ViagemId) : IRequest;
