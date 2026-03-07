using Fleet.Domain.Condutores.Enum;
using MediatR;

namespace Fleet.Application.Condutores.Command;

public sealed record AtualizarCondutorCommand(
    Guid Id,
    StatusCondutorEnum Status
) : IRequest;
