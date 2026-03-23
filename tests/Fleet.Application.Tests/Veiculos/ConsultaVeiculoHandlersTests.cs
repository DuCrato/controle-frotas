using Fleet.Application.Tests.Common;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Application.Veiculos.Query;

namespace Fleet.Application.Tests.Veiculos;

public class ConsultaVeiculoHandlersTests : TestBase
{
    private readonly Mock<IVeiculoRepository> _repositoryMock;

    public ConsultaVeiculoHandlersTests()
    {
        _repositoryMock = new Mock<IVeiculoRepository>();
    }

    [Fact]
    public async Task ObterVeiculoPorIdHandler_WhenVehicleExists_ShouldReturnVehicleDto()
    {
        // Arrange
        var veiculo = VeiculoTestFactory.Criar();
        var query = new ObterVeiculoPorIdQuery(veiculo.Id);
        var handler = new ObterVeiculoPorIdHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        // Act
        var dto = await handler.Handle(query, CancellationToken.None);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(veiculo.Id);
        dto.Placa.Should().Be(veiculo.Placa.Valor);

        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o veículo por ID");
    }

    [Fact]
    public async Task ObterVeiculoPorIdHandler_WhenVehicleNotExists_ShouldReturnNull()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var query = new ObterVeiculoPorIdQuery(veiculoId);
        var handler = new ObterVeiculoPorIdHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Veiculos.Entidades.Veiculo?)null);

        // Act
        var dto = await handler.Handle(query, CancellationToken.None);

        // Assert
        dto.Should().BeNull();
    }

    [Fact]
    public async Task ListagemVeiculosHandler_WhenVehiclesExist_ShouldReturnAllVehicles()
    {
        // Arrange
        var veiculos = new List<Fleet.Domain.Veiculos.Entidades.Veiculo>
        {
            VeiculoTestFactory.Criar(),
            VeiculoTestFactory.Criar("BRA1234", "23456789012345", "9BWZZZ377VT004252", Fixture.Create<string>()),
            VeiculoTestFactory.Criar("CAR5678", "34567890123456", "9BWZZZ377VT004253", Fixture.Create<string>())
        };

        var query = new ListagemVeiculosQuery();
        var handler = new ListagemVeiculosHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculos);

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().HaveCount(3);
        resultado.Should().AllSatisfy(x => x.Should().NotBeNull());

        _repositoryMock.Verify(
            x => x.ListagemAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar todos os veículos");
    }

    [Fact]
    public async Task ListagemVeiculosHandler_WhenNoVehiclesExist_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new ListagemVeiculosQuery();
        var handler = new ListagemVeiculosHandler(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Fleet.Domain.Veiculos.Entidades.Veiculo>());

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeEmpty();
    }
}
