using Fleet.Domain.Condutores.Enum;

namespace Fleet.Application.Condutores.Dto
{
    public record CondutorDto(
        Guid Id,
        string Nome,
        string Cpf,
        string CnhNumero,
        string CnhCategoria,
        DateTime CnhValidade,
        StatusCondutorEnum Status,
        bool PodeSerAlocado
        );
}
