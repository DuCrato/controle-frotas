using Fleet.Application.Viagens.Command;
using Fleet.Application.Viagens.Handler;
using Fleet.Application.Viagens.Interface;
using Fleet.Application.Tests.Common;
using Fleet.Domain.Viagens.ValueObjects;

namespace Fleet.Application.Tests.Viagens;

public class CriarViagemHandlerTests : TestBase
{
    private readonly Mock<IViagemRepository> _viagemRepositoryMock;
    private readonly CriarViagemHandler _handler;

    public CriarViagemHandlerTests()
    {
        _viagemRepositoryMock = new Mock<IViagemRepository>();
        _handler = new CriarViagemHandler(_viagemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommand_ShouldCreateViagemSuccessfully()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var condutorId = Guid.NewGuid();
        var agora = DateTime.UtcNow;

        var command = new CriarViagemCommand(
            VeiculoId: veiculoId,
            CondutorId: condutorId,
            LatitudeOrigem: -23.5505m,
            LongitudeOrigem: -46.6333m,
            EnderecoOrigem: "Rua A, 123",
            LatitudeDestino: -22.9068m,
            LongitudeDestino: -43.1729m,
            EnderecoDestino: "Rua B, 456",
            DataHoraPrevistaSaida: agora.AddHours(2),
            DataHoraPrevistaChegada: agora.AddHours(4),
            DistanciaEstimada: 150.50m,
            Observacoes: "Viagem teste");

        _viagemRepositoryMock
            .Setup(x => x.CondutorTemViagemEmAndamentoAsync(condutorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _viagemRepositoryMock
            .Setup(x => x.VeiculoTemViagemEmAndamentoAsync(veiculoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _viagemRepositoryMock
            .Setup(x => x.CriarAsync(It.IsAny<Fleet.Domain.Viagens.Entidades.Viagem>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _viagemRepositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var id = await _handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBe(Guid.Empty);

        _viagemRepositoryMock.Verify(
            x => x.CondutorTemViagemEmAndamentoAsync(condutorId, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se condutor tem viagem em andamento");

        _viagemRepositoryMock.Verify(
            x => x.VeiculoTemViagemEmAndamentoAsync(veiculoId, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve verificar se veículo tem viagem em andamento");

        _viagemRepositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Viagens.Entidades.Viagem>(), It.IsAny<CancellationToken>()),
            Times.Once,
            "deve criar a viagem");

        _viagemRepositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenCondutorHasActiveTrip_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var condutorId = Guid.NewGuid();
        var agora = DateTime.UtcNow;

        var command = new CriarViagemCommand(
            VeiculoId: veiculoId,
            CondutorId: condutorId,
            LatitudeOrigem: -23.5505m,
            LongitudeOrigem: -46.6333m,
            EnderecoOrigem: "Rua A, 123",
            LatitudeDestino: -22.9068m,
            LongitudeDestino: -43.1729m,
            EnderecoDestino: "Rua B, 456",
            DataHoraPrevistaSaida: agora.AddHours(2),
            DataHoraPrevistaChegada: agora.AddHours(4),
            DistanciaEstimada: 150.50m);

        _viagemRepositoryMock
            .Setup(x => x.CondutorTemViagemEmAndamentoAsync(condutorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Condutor");

        _viagemRepositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Viagens.Entidades.Viagem>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar viagem quando condutor tem viagem ativa");
    }

    [Fact]
    public async Task Handle_WhenVehicleHasActiveTrip_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var condutorId = Guid.NewGuid();
        var agora = DateTime.UtcNow;

        var command = new CriarViagemCommand(
            VeiculoId: veiculoId,
            CondutorId: condutorId,
            LatitudeOrigem: -23.5505m,
            LongitudeOrigem: -46.6333m,
            EnderecoOrigem: "Rua A, 123",
            LatitudeDestino: -22.9068m,
            LongitudeDestino: -43.1729m,
            EnderecoDestino: "Rua B, 456",
            DataHoraPrevistaSaida: agora.AddHours(2),
            DataHoraPrevistaChegada: agora.AddHours(4),
            DistanciaEstimada: 150.50m);

        _viagemRepositoryMock
            .Setup(x => x.CondutorTemViagemEmAndamentoAsync(condutorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _viagemRepositoryMock
            .Setup(x => x.VeiculoTemViagemEmAndamentoAsync(veiculoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Veículo");

        _viagemRepositoryMock.Verify(
            x => x.CriarAsync(It.IsAny<Fleet.Domain.Viagens.Entidades.Viagem>(), It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve criar viagem quando veículo tem viagem ativa");
    }
}
