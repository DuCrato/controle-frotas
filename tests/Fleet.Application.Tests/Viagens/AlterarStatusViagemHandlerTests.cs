using Fleet.Application.Viagens.Command;
using Fleet.Application.Viagens.Handler;
using Fleet.Application.Viagens.Interface;
using Fleet.Application.Tests.Common;

namespace Fleet.Application.Tests.Viagens;

public class AlterarStatusViagemHandlerTests : TestBase
{
    private readonly Mock<IViagemRepository> _viagemRepositoryMock;
    private readonly IniciarViagemHandler _iniciarHandler;
    private readonly ConcluirViagemHandler _concluirHandler;
    private readonly PausarViagemHandler _pausarHandler;
    private readonly RetomarViagemHandler _retomarHandler;
    private readonly CancelarViagemHandler _cancelarHandler;

    public AlterarStatusViagemHandlerTests()
    {
        _viagemRepositoryMock = new Mock<IViagemRepository>();
        _iniciarHandler = new IniciarViagemHandler(_viagemRepositoryMock.Object);
        _concluirHandler = new ConcluirViagemHandler(_viagemRepositoryMock.Object);
        _pausarHandler = new PausarViagemHandler(_viagemRepositoryMock.Object);
        _retomarHandler = new RetomarViagemHandler(_viagemRepositoryMock.Object);
        _cancelarHandler = new CancelarViagemHandler(_viagemRepositoryMock.Object);
    }

    [Fact]
    public async Task IniciarViagemHandler_WhenValidCommand_ShouldStartTripSuccessfully()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        var command = new IniciarViagemCommand(viagem.Id, 100.50m);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _iniciarHandler.Handle(command, CancellationToken.None);

        // Assert
        _viagemRepositoryMock.Verify(
            x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task IniciarViagemHandler_WhenViagemNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var viagemId = Guid.NewGuid();
        var command = new IniciarViagemCommand(viagemId, 100.50m);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Viagens.Entidades.Viagem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _iniciarHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ConcluirViagemHandler_WhenValidCommand_ShouldCompleteTripSuccessfully()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        viagem.Iniciar(100.50m); // Iniciar antes de concluir
        var command = new ConcluirViagemCommand(viagem.Id, 250.75m);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _concluirHandler.Handle(command, CancellationToken.None);

        // Assert
        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PausarViagemHandler_WhenValidCommand_ShouldPauseTripSuccessfully()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        viagem.Iniciar(100.50m);
        var command = new PausarViagemCommand(viagem.Id);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _pausarHandler.Handle(command, CancellationToken.None);

        // Assert
        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RetomarViagemHandler_WhenValidCommand_ShouldResumeTripSuccessfully()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        viagem.Iniciar(100.50m);
        viagem.Pausar();
        var command = new RetomarViagemCommand(viagem.Id);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _retomarHandler.Handle(command, CancellationToken.None);

        // Assert
        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CancelarViagemHandler_WhenValidCommand_ShouldCancelTripSuccessfully()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        var command = new CancelarViagemCommand(viagem.Id);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _cancelarHandler.Handle(command, CancellationToken.None);

        // Assert
        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
