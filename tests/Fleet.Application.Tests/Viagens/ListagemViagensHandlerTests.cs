using Fleet.Application.Viagens.Handler;
using Fleet.Application.Viagens.Interface;
using Fleet.Application.Viagens.Query;
using Fleet.Application.Tests.Common;

namespace Fleet.Application.Tests.Viagens;

public class ListagemViagensHandlerTests : TestBase
{
    private readonly Mock<IViagemRepository> _viagemRepositoryMock;
    private readonly ObterViagemPorIdHandler _obterPorIdHandler;
    private readonly ListagemViagensHandler _listagemHandler;
    private readonly ListagemViagensPorCondutorHandler _listagemCondutorHandler;
    private readonly ListagemViagensPorVeiculoHandler _listagemVeiculoHandler;

    public ListagemViagensHandlerTests()
    {
        _viagemRepositoryMock = new Mock<IViagemRepository>();
        _obterPorIdHandler = new ObterViagemPorIdHandler(_viagemRepositoryMock.Object);
        _listagemHandler = new ListagemViagensHandler(_viagemRepositoryMock.Object);
        _listagemCondutorHandler = new ListagemViagensPorCondutorHandler(_viagemRepositoryMock.Object);
        _listagemVeiculoHandler = new ListagemViagensPorVeiculoHandler(_viagemRepositoryMock.Object);
    }

    [Fact]
    public async Task ObterViagemPorIdHandler_WhenViagemExists_ShouldReturnTripDetails()
    {
        // Arrange
        var viagem = ViagemTestFactory.Criar();
        var query = new ObterViagemPorIdQuery(viagem.Id);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagem);

        // Act
        var resultado = await _obterPorIdHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(viagem.Id);
        resultado.VeiculoId.Should().Be(viagem.VeiculoId);
        resultado.CondutorId.Should().Be(viagem.CondutorId);

        _viagemRepositoryMock.Verify(
            x => x.ObterPorIdAsync(viagem.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ObterViagemPorIdHandler_WhenViagemNotFound_ShouldReturnNull()
    {
        // Arrange
        var viagemId = Guid.NewGuid();
        var query = new ObterViagemPorIdQuery(viagemId);

        _viagemRepositoryMock
            .Setup(x => x.ObterPorIdAsync(viagemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Viagens.Entidades.Viagem?)null);

        // Act
        var resultado = await _obterPorIdHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ListagemViagensHandler_WhenTripsExist_ShouldReturnAllTrips()
    {
        // Arrange
        var viagens = ViagemTestFactory.CriarLista(5);
        var query = new ListagemViagensQuery();

        _viagemRepositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagens);

        // Act
        var resultado = await _listagemHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeEmpty();
        resultado.Should().HaveCount(5);

        _viagemRepositoryMock.Verify(
            x => x.ListagemAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ListagemViagensHandler_WhenNoTripsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new ListagemViagensQuery();

        _viagemRepositoryMock
            .Setup(x => x.ListagemAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var resultado = await _listagemHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().BeEmpty();
    }

    [Fact]
    public async Task ListagemViagensPorCondutorHandler_WhenTripsExist_ShouldReturnCondutorTrips()
    {
        // Arrange
        var condutorId = Guid.NewGuid();
        var viagens = ViagemTestFactory.CriarListaPorCondutor(condutorId, 3);
        var query = new ListagemViagensPorCondutorQuery(condutorId);

        _viagemRepositoryMock
            .Setup(x => x.ListagemPorCondutorAsync(condutorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagens);

        // Act
        var resultado = await _listagemCondutorHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeEmpty();
        resultado.Should().HaveCount(3);
        resultado.All(v => v.CondutorId == condutorId).Should().BeTrue();

        _viagemRepositoryMock.Verify(
            x => x.ListagemPorCondutorAsync(condutorId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ListagemViagensPorVeiculoHandler_WhenTripsExist_ShouldReturnVehicleTrips()
    {
        // Arrange
        var veiculoId = Guid.NewGuid();
        var viagens = ViagemTestFactory.CriarListaPorVeiculo(veiculoId, 3);
        var query = new ListagemViagensPorVeiculoQuery(veiculoId);

        _viagemRepositoryMock
            .Setup(x => x.ListagemPorVeiculoAsync(veiculoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(viagens);

        // Act
        var resultado = await _listagemVeiculoHandler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeEmpty();
        resultado.Should().HaveCount(3);
        resultado.All(v => v.VeiculoId == veiculoId).Should().BeTrue();

        _viagemRepositoryMock.Verify(
            x => x.ListagemPorVeiculoAsync(veiculoId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
