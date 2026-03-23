namespace Fleet.Domain.Viagens.Enum;

/// <summary>
/// Estados possíveis de uma viagem.
/// </summary>
public enum StatusViagemEnum
{
    /// <summary>
    /// Viagem planejada, não iniciada
    /// </summary>
    Planejada = 0,

    /// <summary>
    /// Viagem em andamento
    /// </summary>
    EmAndamento = 1,

    /// <summary>
    /// Viagem completada com sucesso
    /// </summary>
    Concluida = 2,

    /// <summary>
    /// Viagem cancelada
    /// </summary>
    Cancelada = 3,

    /// <summary>
    /// Viagem pausada
    /// </summary>
    Pausada = 4
}
