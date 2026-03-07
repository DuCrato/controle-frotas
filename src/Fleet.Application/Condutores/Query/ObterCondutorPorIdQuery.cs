using Fleet.Application.Condutores.Dto;
using MediatR;

namespace Fleet.Application.Condutores.Query;

public sealed record ObterCondutorPorIdQuery(Guid Id) : IRequest<CondutorDto>;
