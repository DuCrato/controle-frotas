using Fleet.Domain.Veiculos.Entidades;
using Fleet.Domain.Veiculos.Enum;
using Fleet.Domain.Veiculos.ValueObjects;

namespace Fleet.Application.Tests.Veiculos;

/// <summary>
/// Factory para criar instâncias de teste de Veiculo com dados padrão e customizáveis.
/// Segue o padrão Object Mother para facilitar testes.
/// </summary>
internal static class VeiculoTestFactory
{
    private static long _placaCounter = 1000;
    private static long _renavamCounter = 10000000000000L;
    private static long _chassiCounter = 1000000000000000L;

    public static Veiculo Criar(
        string placa = "ABC1234",
        string renavam = "12345678901234",
        string chassi = "9BWZZZ377VT004251",
        string nome = "Maria Souza",
        StatusVeiculoEnum status = StatusVeiculoEnum.Ativo,
        string estado = "SP",
        string cidade = "São Paulo")
    {
        return new Veiculo(
            new Placa(placa),
            new Renavam(renavam),
            new Chassi(chassi),
            nome,
            status,
            new Endereco(estado, cidade));
    }

    /// <summary>
    /// Cria um veículo com valores aleatórios e únicos.
    /// </summary>
    public static Veiculo CriarAleatorio()
    {
        var placa = $"ABC{(_placaCounter++):D4}";
        var renavam = (_renavamCounter++).ToString("00000000000000");
        var chassi = (_chassiCounter++).ToString("0000000000000000");

        return new Veiculo(
            new Placa(placa),
            new Renavam(renavam),
            new Chassi(chassi),
            $"Proprietário {Guid.NewGuid():N}",
            StatusVeiculoEnum.Ativo,
            new Endereco("SP", "São Paulo"));
    }

    /// <summary>
    /// Cria múltiplos veículos para testes de listagem.
    /// </summary>
    public static List<Veiculo> CriarLista(int quantidade)
    {
        return Enumerable
            .Range(1, quantidade)
            .Select(_ => CriarAleatorio())
            .ToList();
    }
}
