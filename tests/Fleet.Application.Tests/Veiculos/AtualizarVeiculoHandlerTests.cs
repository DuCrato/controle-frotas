using Fleet.Application.Tests.Common;
using Fleet.Application.Veiculos.Command;
using Fleet.Application.Veiculos.Handler;
using Fleet.Application.Veiculos.Interface;
using Fleet.Domain.Veiculos.Enum;

namespace Fleet.Application.Tests.Veiculos;

public class AtualizarVeiculoHandlerTests : TestBase
{
    private readonly Mock<IVeiculoRepository> _repositoryMock;
    private readonly AtualizarVeiculoHandler _handler;

    public AtualizarVeiculoHandlerTests()
    {
        _repositoryMock = new Mock<IVeiculoRepository>();
        _handler = new AtualizarVeiculoHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenVehicleFound_ShouldUpdateSuccessfully()
    {
        // Arrange
        var veiculo = VeiculoTestFactory.Criar();
        var novoProprietario = "Ana Costa";
        var novoStatus = StatusVeiculoEnum.EmManutencao;
        var novoEstado = "RJ";
        var novaCidade = "Rio de Janeiro";

        var command = new AtualizarVeiculoCommand(
            veiculo.Id,
            novoProprietario,
            novoStatus,
            novoEstado,
            novaCidade);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        veiculo.NomeProprietario.Should().Be(novoProprietario);
        veiculo.Status.Should().Be(novoStatus);
        veiculo.Endereco.Estado.Should().Be(novoEstado);
        veiculo.Endereco.Cidade.Should().Be(novaCidade);

        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o veículo");

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
        var command = new AtualizarVeiculoCommand(
            veiculoId,
            Fixture.Create<string>(),
            StatusVeiculoEnum.Ativo,
            "SP",
            "São Paulo");

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Veiculos.Entidades.Veiculo?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain(veiculoId.ToString());

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve salvar alterações para veículo não encontrado");
    }

    [Fact]
    public async Task Handle_WhenChangingMultipleFields_ShouldUpdateAll()
    {
        // Arrange
        var veiculo = VeiculoTestFactory.Criar();
        var command = new AtualizarVeiculoCommand(
            veiculo.Id,
            "João da Silva",
            StatusVeiculoEnum.Inativo,
            "MG",
            "Belo Horizonte");

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(veiculo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(veiculo);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        veiculo.NomeProprietario.Should().Be("João da Silva");
        veiculo.Status.Should().Be(StatusVeiculoEnum.Inativo);
        veiculo.Endereco.Estado.Should().Be("MG");
        veiculo.Endereco.Cidade.Should().Be("Belo Horizonte");
    }
}
