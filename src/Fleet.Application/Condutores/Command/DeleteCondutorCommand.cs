using MediatR;

namespace Fleet.Application.Condutores.Command;

public sealed record DeleteCondutorCommand(Guid Id) : IRequest;
