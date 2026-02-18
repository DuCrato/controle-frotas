namespace Fleet.Application.Veiculos.Dtos
{
    public sealed record VeiculoDto(
        Guid Id,
        string Placa,
        string Renavam,
        string Chassi,
        string NomeProprietario,
        string Status,
        string Estado,
        string Cidade
    );
}
