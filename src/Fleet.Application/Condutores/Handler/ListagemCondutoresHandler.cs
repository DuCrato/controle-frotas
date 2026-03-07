using Fleet.Application.Condutores.Dto;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Condutores.Query;
using MediatR;

namespace Fleet.Application.Condutores.Handler;

public sealed class ListagemCondutoresHandler(ICondutorRepository repository) : IRequestHandler<ListagemCondutoresQuery, List<CondutorDto>>
{
    public async Task<List<CondutorDto>> Handle(ListagemCondutoresQuery request, CancellationToken cancellationToken)
    {
        var condutores = await repository.ListagemAsync(cancellationToken);

        return [.. condutores.Select(c => new CondutorDto(
            c.Id,
            c.Nome,
            c.Cpf,
            c.Cnh.Numero,
            c.Cnh.Categoria,
            c.Cnh.Validade,
            c.Status,
            c.PodeSerAlocado()
        ))];
    }
}
