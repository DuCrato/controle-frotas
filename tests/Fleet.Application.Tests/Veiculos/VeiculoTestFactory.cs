using Fleet.Domain.Veiculos.Entidades;
using Fleet.Domain.Veiculos.Enumeradores;
using Fleet.Domain.Veiculos.ValueObjects;

namespace Fleet.Application.Tests.Veiculos;

internal static class VeiculoTestFactory
{
    public static Veiculo Criar(
        string placa = "ABC1234",
        string renavam = "12345678901",
        string chassi = "9BWZZZ377VT004251",
        string nome = "Maria Souza",
        StatusVeiculoEnum status = StatusVeiculoEnum.Ativo,
        string estado = "SP",
        string cidade = "Sao Paulo")
    {
        return new Veiculo(
            new Placa(placa),
            new Renavam(renavam),
            new Chassi(chassi),
            nome,
            status,
            new Endereco(estado, cidade));
    }
}
