using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Application.Veiculos.Query;
using Moq;

namespace Fleet.Application.Tests.Veiculos;

public class ConsultaVeiculoHandlersTests
{
    [Fact]
    public async Task ObterPorId_DeveRetornarDto_QuandoVeiculoExistir()
    {
        var veiculo = VeiculoTestFactory.Criar();
        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(veiculo);

        var handler = new ObterVeiculoPorIdHandler(repository.Object);

        var dto = await handler.Handle(new ObterVeiculoPorIdQuery(veiculo.Id), CancellationToken.None);

        Assert.Equal(veiculo.Id, dto.Id);
        Assert.Equal(veiculo.Placa.Valor, dto.Placa);
    }

    [Fact]
    public async Task Listagem_DeveRetornarDtos_QuandoExistiremVeiculos()
    {
        var veiculos = new List<Fleet.Domain.Veiculos.Entidades.Veiculo>
        {
            VeiculoTestFactory.Criar(),
            VeiculoTestFactory.Criar("BRA1234", "23456789012", "9BWZZZ377VT004252", "Carlos Lima")
        };

        var repository = new Mock<IVeiculoRepository>();
        repository.Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>())).ReturnsAsync(veiculos);

        var handler = new ListagemVeiculosHandler(repository.Object);

        var resultado = await handler.Handle(new ListagemVeiculosQuery(), CancellationToken.None);

        Assert.Equal(2, resultado.Count);
    }
}
