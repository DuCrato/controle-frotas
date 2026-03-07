using Fleet.Domain.Condutores.Entidades;
using Fleet.Domain.Condutores.Enum;
using Fleet.Domain.Condutores.ValueObjects;

namespace Fleet.Application.Tests.Condutores;

internal static class CondutorTestFactory
{
    public static Condutor Criar(
        string nome = "Joao Silva",
        string cpf = "12345678901",
        string numeroCnh = "12345678900",
        string categoria = "B",
        DateTime? validade = null,
        StatusCondutorEnum status = StatusCondutorEnum.Ativo)
    {
        return new Condutor(
            nome,
            cpf,
            new Cnh(numeroCnh, categoria, validade ?? DateTime.Today.AddYears(2)),
            status);
    }
}
