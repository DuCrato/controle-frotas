using Fleet.Application.Tests.Common;
using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Enum;

namespace Fleet.Application.Tests.Veiculos;

public class CriarVeiculoHandlerTests : TestBase
{
    private readonly Mock<IVeiculoRepository> _repositoryMock;
    private readonly CriarVeiculoHandler _handler;

    public CriarVeiculoHandlerTests()
    {
        _repositoryMock = new Mock<IVeiculoRepository>();
        _handler = new CriarVeiculoHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAllIdentifiersAreUnique_ShouldCreateVehicleSuccessfully()
    {
        // Arrange
        var command = new CriarVeiculoCommand(
            "ABC1234",
            "12345678901234",
            "9BWZZZ377VT004251",
            Fixture.Create<string>(),
            StatusVeiculoEnum.Ativo,
            "SP",
            "São Paulo");

        _repositoryMock
            .Setup(x => x.ExistePorPlacaAsync(command.Placa, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.ExistePorRenavamAsync(command.Renavam, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.ExistePorChassiAsync(command.Chassi, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var id = await _handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBe(Guid.Empty);

        _repositoryMock.Verify(
            x => x.ExistePorPlacaAsync(command.Placa, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se placa já existe");

        _repositoryMock.Verify(
            x => x.ExistePorRenavamAsync(command.Renavam, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se renavam já existe");

        _repositoryMock.Verify(
            x => x.ExistePorChassiAsync(command.Chassi, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se chassi já existe");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Once,
            "deve criar o veículo");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenPlateAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var placa = "ABC1234";
        var command = new CriarVeiculoCommand(
            placa,
            "12345678901234",
            "9BWZZZ377VT004251",
            Fixture.Create<string>(),
            StatusVeiculoEnum.Ativo,
            "SP",
            "São Paulo");

        _repositoryMock
            .Setup(x => x.ExistePorPlacaAsync(placa, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("placa");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar veículo com placa duplicada");
    }

    [Fact]
    public async Task Handle_WhenRenavamAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var renavam = "12345678901234";
        var command = new CriarVeiculoCommand(
            "ABC1234",
            renavam,
            "9BWZZZ377VT004251",
            Fixture.Create<string>(),
            StatusVeiculoEnum.Ativo,
            "SP",
            "São Paulo");

        _repositoryMock
            .Setup(x => x.ExistePorPlacaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.ExistePorRenavamAsync(renavam, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("renavam");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar veículo com renavam duplicado");
    }

    [Fact]
    public async Task Handle_WhenChassisAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var chassi = "9BWZZZ377VT004251";
        var command = new CriarVeiculoCommand(
            "ABC1234",
            "12345678901234",
            chassi,
            Fixture.Create<string>(),
            StatusVeiculoEnum.Ativo,
            "SP",
            "São Paulo");

        _repositoryMock
            .Setup(x => x.ExistePorPlacaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.ExistePorRenavamAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.ExistePorChassiAsync(chassi, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("chassi");

        _repositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar veículo com chassi duplicado");
    }
}
