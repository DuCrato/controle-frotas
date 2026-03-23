using Fleet.Domain.Viagens.Entidades;
using Fleet.Domain.Viagens.ValueObjects;

namespace Fleet.Application.Tests.Viagens;

/// <summary>
/// Factory para criar instâncias de teste de Viagem com dados padrão e customizáveis.
/// Segue o padrão Object Mother para facilitar testes.
/// </summary>
internal static class ViagemTestFactory
{
    /// <summary>
    /// Cria uma viagem com valores padrão, permitindo sobrescrita de campos específicos.
    /// </summary>
    public static Viagem Criar(
        Guid? veiculoId = null,
        Guid? condutorId = null,
        decimal? latitudeOrigem = null,
        decimal? longitudeOrigem = null,
        string enderecoOrigem = "Rua A, 123",
        decimal? latitudeDestino = null,
        decimal? longitudeDestino = null,
        string enderecoDestino = "Rua B, 456",
        DateTime? dataSaida = null,
        DateTime? dataChegada = null,
        decimal distanciaEstimada = 150.50m,
        string? observacoes = null)
    {
        var origem = new Localizacao(
            latitudeOrigem ?? -23.5505m,
            longitudeOrigem ?? -46.6333m,
            enderecoOrigem);

        var destino = new Localizacao(
            latitudeDestino ?? -22.9068m,
            longitudeDestino ?? -43.1729m,
            enderecoDestino);

        var agora = DateTime.UtcNow;
        var saida = dataSaida ?? agora.AddHours(2);
        var chegada = dataChegada ?? agora.AddHours(4);

        return new Viagem(
            veiculoId ?? Guid.NewGuid(),
            condutorId ?? Guid.NewGuid(),
            origem,
            destino,
            saida,
            chegada,
            distanciaEstimada,
            observacoes);
    }

    /// <summary>
    /// Cria uma viagem com valores aleatórios.
    /// </summary>
    public static Viagem CriarAleatorio()
    {
        var agora = DateTime.UtcNow;
        var numeroAleatorio = Guid.NewGuid().GetHashCode();
        var distancia = 100m + ((Math.Abs(numeroAleatorio) % 200) * 1.5m);

        return new Viagem(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new Localizacao(-23.5505m, -46.6333m, $"Origem {numeroAleatorio}"),
            new Localizacao(-22.9068m, -43.1729m, $"Destino {numeroAleatorio}"),
            agora.AddHours(2),
            agora.AddHours(4),
            distancia,
            $"Observações {numeroAleatorio}");
    }

    /// <summary>
    /// Cria múltiplas viagens para testes de listagem.
    /// </summary>
    public static List<Viagem> CriarLista(int quantidade)
    {
        return Enumerable
            .Range(1, quantidade)
            .Select(_ => CriarAleatorio())
            .ToList();
    }

    /// <summary>
    /// Cria viagens de um condutor específico.
    /// </summary>
    public static List<Viagem> CriarListaPorCondutor(Guid condutorId, int quantidade)
    {
        return Enumerable
            .Range(1, quantidade)
            .Select(_ => Criar(condutorId: condutorId))
            .ToList();
    }

    /// <summary>
    /// Cria viagens de um veículo específico.
    /// </summary>
    public static List<Viagem> CriarListaPorVeiculo(Guid veiculoId, int quantidade)
    {
        return Enumerable
            .Range(1, quantidade)
            .Select(_ => Criar(veiculoId: veiculoId))
            .ToList();
    }
}
