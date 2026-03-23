using Fleet.Application.Tests.Common;
using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;

namespace Fleet.Application.Tests.Veiculos;

public class DeleteVeiculoHandlerTests : TestBase
{
    private readonly Mock<IVeiculoRepository> _repositoryMock;
    private readonly DeleteVeiculoHandler _handler;

    public DeleteVeiculoHandlerTests()
    {
        _repositoryMock = new Mock<IVeiculoRepository>();
        _handler = new DeleteVeiculoHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenVehicleFound_ShouldDeleteSuccessfully()
    {
        // Arrange
        var veiculo = VeiculoTestFactory.Criar();
        var command = new DeleteVeiculoCommand(veiculo.Id);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o veículo");

        _repositoryMock.Verify(
            x => x.Delete(veiculo),
            Times.Once,
            "deve deletar o veículo");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var command = new DeleteVeiculoCommand(veiculoId);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Veiculos.Entidades.Veiculo?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain(veiculoId.ToString());

        _repositoryMock.Verify(
            x => x.Delete(It.IsAny<Fleet.Domain.Veiculos.Entidades.Veiculo>()),
            Times.Never,
            "não deve deletar veículo não encontrado");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve salvar alterações para veículo não encontrado");
    }

    [Fact]
    public async Task Handle_WhenSaveChangesFails_ShouldThrowException()
    {
        // Arrange
        var veiculo = VeiculoTestFactory.Criar();
        var command = new DeleteVeiculoCommand(veiculo.Id);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Database error", new Exception()));

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.Delete(veiculo),
            Times.Once,
            "deve tentar deletar mesmo se salvar falhar");
    }
}
