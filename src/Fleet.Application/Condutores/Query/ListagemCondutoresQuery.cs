using Fleet.Application.Condutores.Dto;
using MediatR;

namespace Fleet.Application.Condutores.Query;

public sealed record ListagemCondutoresQuery() : IRequest<List<CondutorDto>>;
