using Fleet.Domain.Condutores.Entidades;
using Fleet.Domain.Condutores.Enum;
using Fleet.Domain.Condutores.ValueObjects;

namespace Fleet.Application.Tests.Condutores;

/// <summary>
/// Factory para criar instâncias de teste de Condutor com dados padrão e customizáveis.
/// Segue o padrão Object Mother para facilitar testes.
/// </summary>
internal static class CondutorTestFactory
{
    private static int _cpfCounter = 10000000000;
    private static int _cnhCounter = 10000000000;

    /// <summary>
    /// Cria um condutor com valores padrão, permitindo sobrescrita de campos específicos.
    /// </summary>
    public static Condutor Criar(
        string nome = "João da Silva",
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

    /// <summary>
    /// Cria um condutor com valores aleatórios e únicos.
    /// </summary>
    public static Condutor CriarAleatorio()
    {
        var cpf = (_cpfCounter++).ToString("00000000000");
        var cnh = (_cnhCounter++).ToString("00000000000");

        return new Condutor(
            $"Condutor Teste {Guid.NewGuid():N}",
            cpf,
            new Cnh(cnh, "B", DateTime.Today.AddYears(2)),
            StatusCondutorEnum.Ativo);
    }

    /// <summary>
    /// Cria múltiplos condutores para testes de listagem.
    /// </summary>
    public static List<Condutor> CriarLista(int quantidade)
    {
        return Enumerable
            .Range(1, quantidade)
            .Select(_ => CriarAleatorio())
            .ToList();
    }
}
