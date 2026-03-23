using Fleet.Application.Condutores.Command;
using Fleet.Application.Condutores.Handler;
using Fleet.Application.Condutores.Interface;
using Fleet.Application.Tests.Common;
using Fleet.Domain.Condutores.Enum;

namespace Fleet.Application.Tests.Condutores;

public class AtualizarCondutorHandlerTests : TestBase
{
    private readonly Mock<ICondutorRepository> _repositoryMock;
    private readonly AtualizarCondutorHandler _handler;

    public AtualizarCondutorHandlerTests()
    {
        _repositoryMock = new Mock<ICondutorRepository>();
        _handler = new AtualizarCondutorHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenSuspendingActiveConductor_ShouldUpdateStatusSuccessfully()
    {
        // Arrange
        var condutor = CondutorTestFactory.Criar(status: StatusCondutorEnum.Ativo);
        var command = new AtualizarCondutorCommand(condutor.Id, StatusCondutorEnum.Suspenso);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutor);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        condutor.Status.Should().Be(StatusCondutorEnum.Suspenso);

        _repositoryMock.Verify(
            x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()),
            Times.Once,
            "deve buscar o condutor");

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar as alterações");
    }

    [Fact]
    public async Task Handle_WhenConductorNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var conductorId = Guid.NewGuid();
        var command = new AtualizarCondutorCommand(conductorId, StatusCondutorEnum.Ativo);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fleet.Domain.Condutores.Entidades.Condutor?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain(conductorId.ToString());

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "não deve salvar alterações para condutor não encontrado");
    }

    [Fact]
    public async Task Handle_WhenReactivatingConductorWithValidLicense_ShouldSucceed()
    {
        // Arrange
        var futureDate = DateTime.Today.AddYears(2);
        var condutor = CondutorTestFactory.Criar(status: StatusCondutorEnum.Inativo, validade: futureDate);
        var command = new AtualizarCondutorCommand(condutor.Id, StatusCondutorEnum.Ativo);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(condutor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(condutor);

        _repositoryMock
            .Setup(x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        condutor.Status.Should().Be(StatusCondutorEnum.Ativo);

        _repositoryMock.Verify(
            x => x.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "deve salvar alterações com CNH válida");
    }
}
