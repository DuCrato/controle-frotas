using Fleet.Domain.Condutores.Enum;
using MediatR;

namespace Fleet.Application.Condutores.Command
{
    public sealed record CriarCondutorCommand(
        string Nome,
        string Cpf,
        string CnhNumero,
        string CnhCategoria,
        DateTime CnhValidade,
        StatusCondutorEnum Status
    ) : IRequest<Guid>;
}
