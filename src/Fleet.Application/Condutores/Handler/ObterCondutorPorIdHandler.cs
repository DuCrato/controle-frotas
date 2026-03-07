using Fleet.Application.Condutores.Dto;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Condutores.Query;
using MediatR;

namespace Fleet.Application.Condutores.Handler;

public sealed class ObterCondutorPorIdHandler(ICondutorRepository repository) : IRequestHandler<ObterCondutorPorIdQuery, CondutorDto>
{
    public async Task<CondutorDto> Handle(ObterCondutorPorIdQuery request, CancellationToken cancellationToken)
    {
        var condutor = await repository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Condutor não encontrado.");

        return new CondutorDto(
            condutor.Id,
            condutor.Nome,
            condutor.Cpf,
            condutor.Cnh.Numero,
            condutor.Cnh.Categoria,
            condutor.Cnh.Validade,
            condutor.Status,
            condutor.PodeSerAlocado());
    }
}
